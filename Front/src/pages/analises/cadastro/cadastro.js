// --- ESTRUTURA DE DADOS REESTRUTURADA ---
window.uvVisChart = null;
// Cada dataset agora contém seus próprios pontos {x, y}
window.chartData = {
    datasets: []
};
window.rotuloX = "Eixo X";
window.rotuloY = "Eixo Y";
const lineColors = ['#2b2be8', '#e82b2b', '#2be82b', '#e89c2b', '#9c2be8', '#2be8e8'];

// --- FUNÇÕES DE UI ---

function updateEditableLegend() {
    const container = document.getElementById('line-legend-container');
    container.innerHTML = '<h4>Legenda Editável</h4>';
    window.chartData.datasets.forEach((dataset, index) => {
        const item = document.createElement('div');
        item.className = 'legend-item';
        const colorPicker = document.createElement('input');
        colorPicker.type = 'color';
        colorPicker.value = dataset.borderColor || lineColors[index % lineColors.length];
        colorPicker.addEventListener('input', (e) => {
            const chartDataset = window.uvVisChart.data.datasets.find(d => d.label === dataset.label);
            if (chartDataset) {
                chartDataset.borderColor = e.target.value;
                window.uvVisChart.update();
            }

            const sourceDataset = window.chartData.datasets.find(d => d.label === dataset.label);
            if (sourceDataset) {
                sourceDataset.borderColor = e.target.value;
            }
        });
        const label = document.createElement('span');
        label.textContent = dataset.label;
        item.appendChild(colorPicker);
        item.appendChild(label);
        container.appendChild(item);
    });
}

function addLineFromFile(file, nomeDigitado) {
    const defaultLineName = nomeDigitado || file.name.split('.').slice(0, -1).join('.');

    const processLineData = (data) => {
        if (data.length < 1 || data[0].length < 2) {
            alert("O arquivo da linha precisa ter pelo menos duas colunas (X e Y).");
            return;
        }
        const hasHeader = isNaN(parseFloat(String(data[0][0]).replace(',', '.')));
        const dataRows = hasHeader ? data.slice(1) : data;

        const newData = dataRows.map(row => ({
            x: parseFloat(String(row[0]).replace(',', '.')),
            y: parseFloat(String(row[1]).replace(',', '.'))
        }));

        if (newData.some(p => isNaN(p.x) || isNaN(p.y))) {
            alert("O arquivo da linha contém valores inválidos. Verifique se as duas primeiras colunas contêm apenas números.");
            return;
        }

        window.chartData.datasets.push({ label: defaultLineName, data: newData });
        gerarGrafico();
        alert(`Linha "${defaultLineName}" adicionada com sucesso!`);
    };

    if (file.name.toLowerCase().endsWith('.csv')) {
        Papa.parse(file, { header: false, skipEmptyLines: true, delimiter: ";", complete: (results) => processLineData(results.data) });
    } else if (file.name.toLowerCase().endsWith('.xlsx') || file.name.toLowerCase().endsWith('.xls')) {
        const reader = new FileReader();
        reader.onload = (e) => {
            const workbook = XLSX.read(new Uint8Array(e.target.result), { type: 'array' });
            const sheet = workbook.Sheets[workbook.SheetNames[0]];
            processLineData(XLSX.utils.sheet_to_json(sheet, { header: 1 }));
        };
        reader.readAsArrayBuffer(file);
    }
}

// --- FUNÇÕES DO GRÁFICO ---

function gerarGrafico() {
    const canvas = document.getElementById('uvVisChart');
    const placeholder = document.getElementById('placeholder');
    if (!window.chartData.datasets.length) {
        placeholder.style.display = 'block';
        canvas.style.display = 'none';
        updateEditableLegend();
        return;
    }
    placeholder.style.display = 'none';
    canvas.style.display = 'block';
    if (window.uvVisChart) window.uvVisChart.destroy();

    const finalDatasets = [];
    let verticalOffset = 0;

    // Itera sobre cada linha de dados para calcular seu deslocamento e criar os datasets para o gráfico
    window.chartData.datasets.forEach((dataset, index) => {
        const yValues = dataset.data.map(p => p.y);
        if (yValues.length === 0) return;

        const localMinY = Math.min(...yValues);
        const localMaxY = Math.max(...yValues);

        // O deslocamento move a linha atual para que seu ponto mais baixo comece acima do ponto mais alto anterior
        const shiftAmount = verticalOffset - localMinY;

        // Cria um novo array de dados com os valores Y deslocados
        const transformedData = dataset.data.map(p => ({ x: p.x, y: p.y + shiftAmount }));

        const color = dataset.borderColor || lineColors[index % lineColors.length];
        dataset.borderColor = color;
        finalDatasets.push({
            label: dataset.label,
            data: transformedData,
            borderColor: color,
            borderWidth: 2,
            fill: false,
            tension: 0.1,
            pointRadius: 0
        });

        // Encontra e desloca o ponto de pico também
        const maxValor = Math.max(...yValues);
        const maxIndex = yValues.indexOf(maxValor);
        const picoData = [{ x: dataset.data[maxIndex].x, y: maxValor + shiftAmount }];

        finalDatasets.push({
            label: `Banda (${dataset.label})`,
            data: picoData,
            backgroundColor: 'black',
            pointRadius: 6,
            showLine: false
        });

        // Atualiza o offset para a próxima linha, adicionando uma pequena margem (5% da altura da linha atual)
        const range = localMaxY - localMinY;
        verticalOffset += range + (range * 0.05);
    });

    window.uvVisChart = new Chart(canvas.getContext('2d'), {
        type: 'line',
        data: { datasets: finalDatasets },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    // Callback para garantir que a dica do mouse mostre o valor Y ORIGINAL, não o deslocado
                    callbacks: {
                        label: function(tooltipItem) {
                            // Ignora os pontos de pico na dica
                            if (tooltipItem.dataset.label.startsWith('Banda')) {
                                return null;
                            }
                            // Encontra o índice do dataset original (já que temos linha + pico)
                            const originalDatasetIndex = Math.floor(tooltipItem.datasetIndex / 2);
                            const originalPoint = window.chartData.datasets[originalDatasetIndex].data[tooltipItem.dataIndex];
                            
                            if (originalPoint) {
                                // Formata a dica para mostrar o nome da linha e seu valor Y real
                                return `${tooltipItem.dataset.label}: ${originalPoint.y.toFixed(4)}`;
                            }
                            return '';
                        }
                    }
                }
            },
            scales: {
                x: { type: 'linear', title: { display: true, text: window.rotuloX }, reverse: true, ticks: { maxTicksLimit: 40 } },
                y: {
                    title: { display: true, text: window.rotuloY },
                    // Oculta os rótulos do eixo Y, pois a escala agora é artificial para criar a separação
                    ticks: {
                        display: false
                    }
                }
            }
        }
    });
    updateEditableLegend();
}

function resetAnalysis() {
    document.getElementById('project-name').value = '';
    document.getElementById('project-description').value = '';
    document.getElementById('fileInput').value = '';
    window.chartData = { datasets: [] };
    if (window.uvVisChart) window.uvVisChart.destroy();
    document.getElementById('uvVisChart').style.display = 'none';
    document.getElementById('placeholder').style.display = 'block';
    updateEditableLegend();
}

// --- LÓGICA DE ARQUIVOS E EVENTOS ---

window.addEventListener('DOMContentLoaded', function() {
    const fileInput = document.getElementById('fileInput');
    document.getElementById('btnUpload').addEventListener('click', () => fileInput.click());
    
    const nameInput = document.getElementById('new-line-name');
    const btnAddLineFile = document.getElementById('btn-add-line-file');
    const addLineFileInput = document.getElementById('add-line-file-input');
    
    btnAddLineFile.addEventListener('click', () => addLineFileInput.click());

    addLineFileInput.addEventListener('change', (event) => {
        const file = event.target.files[0];
        if (file) {
            addLineFromFile(file, nameInput.value.trim());
        }
        nameInput.value = '';
        event.target.value = '';
    });

    fileInput.addEventListener('change', function(event) {
        const file = event.target.files[0];
        if (!file) return;

        // **FUNÇÃO DE PROCESSAMENTO TOTALMENTE ATUALIZADA**
        const processData = (data, file) => {
            if (data.length < 4) { 
                alert("O arquivo parece ter um formato inválido. São necessárias pelo menos 4 linhas para títulos, unidades, nomes das amostras e dados."); 
                return; 
            }
            
            // Constrói os rótulos dos eixos a partir das duas primeiras linhas
            const xTitle = data[0][0] || "Eixo X";
            const xUnit = data[1][0] || "";
            window.rotuloX = `${xTitle} ${xUnit}`.trim();

            const yTitle = data[0][1] || "Eixo Y";
            const yUnit = data[1][1] || "";
            window.rotuloY = `${yTitle} ${yUnit}`.trim();
            
            // Pega os nomes das linhas da terceira linha (índice 2)
            const lineNames = data[2]; 
            // Os dados numéricos começam da quarta linha (índice 3)
            const dataRows = data.slice(3); 
            
            const xValues = dataRows.map(row => parseFloat(String(row[0]).replace(',', '.')));
            if (xValues.some(isNaN)) {
                alert(`A coluna de dados do Eixo X (Coluna A, a partir da linha 4) contém valores inválidos.`);
                resetAnalysis();
                return;
            }

            // Itera sobre as colunas de Y, começando da segunda coluna (índice 1)
            for (let i = 1; i < lineNames.length; i++) {
                const datasetLabel = lineNames[i];
                
                // Pula colunas que não têm um nome de amostra na terceira linha
                if (!datasetLabel || String(datasetLabel).trim() === '') continue;

                const yValues = dataRows.map(row => parseFloat(String(row[i]).replace(',', '.')));
                
                if (yValues.some(isNaN)) { 
                    alert(`A coluna da amostra "${datasetLabel}" contém valores inválidos!`); 
                    resetAnalysis(); 
                    return; 
                }
                
                const datasetData = xValues.map((x, index) => ({ x: x, y: yValues[index] }));
                window.chartData.datasets.push({ label: datasetLabel, data: datasetData });
            }
            
            if (window.chartData.datasets.length === 0) {
                alert("Nenhuma coluna de dados Y válida foi encontrada. Verifique se os nomes das amostras estão na terceira linha, a partir da coluna B.");
                return;
            }

            gerarGrafico();
        };

        if (file.name.toLowerCase().endsWith('.csv')) {
            Papa.parse(file, { header: false, skipEmptyLines: true, delimiter: ";", complete: (results) => processData(results.data, file) });
        } else if (file.name.toLowerCase().endsWith('.xlsx') || file.name.toLowerCase().endsWith('.xls')) {
            const reader = new FileReader();
            reader.onload = (e) => {
                const workbook = XLSX.read(new Uint8Array(e.target.result), { type: 'array' });
                const sheet = workbook.Sheets[workbook.SheetNames[0]];
                processData(XLSX.utils.sheet_to_json(sheet, { header: 1 }), file);
            };
            reader.readAsArrayBuffer(file);
        }
    });
    document.getElementById('btnExport')?.addEventListener('click', exportCSV);
});

const togglePointsBtn = document.getElementById('togglePointsBtn');
let picosVisiveis = false;
togglePointsBtn.addEventListener('click', () => {
    if (!window.uvVisChart) return;
    picosVisiveis = !picosVisiveis;
    window.uvVisChart.data.datasets.forEach(dataset => {
        if (dataset.label && !dataset.label.startsWith('Banda')) {
            dataset.pointRadius = picosVisiveis ? 3 : 0;
            dataset.pointBackgroundColor = picosVisiveis ? dataset.borderColor : 'transparent';
        }
    });
    togglePointsBtn.innerHTML = picosVisiveis ? '<i class="fas fa-search-minus"></i> Ocultar Bandas' : '<i class="fas fa-search-plus"></i> Mostrar Bandas';
    window.uvVisChart.update();
});

async function saveAnalysis() {
    const saveButton = document.getElementById('btnSave');
    const originalButtonContent = saveButton.innerHTML;
    saveButton.disabled = true;
    saveButton.innerHTML = `Salvando <span class="loading-dots"></span>`;
    try {
        const token = localStorage.getItem('token');
        if (!token) { alert('Você precisa estar logado para salvar.'); return; }
        const nome = document.getElementById('project-name').value.trim();
        const descricao = document.getElementById('project-description').value.trim();
        if (!window.chartData.datasets.length) { alert("Não há dados para salvar."); return; }
        
        // ALTERAÇÃO: Adiciona a propriedade 'Cor' ao mapear as linhas para salvar.
        const linhas = window.chartData.datasets.map(dataset => ({
            Nome: String(dataset.label),
            Cor: dataset.borderColor, 
            Dados: dataset.data.map(p => ({ ValorX: p.x, ValorY: p.y }))
        }));

        console.log("Linhas a serem salvas: ", linhas);

        const analiseParaSalvar = { Nome: nome, Descricao: descricao, RotuloX: window.rotuloX, RotuloY: window.rotuloY, Linhas: linhas };
        const response = await fetch("http://localhost:5000/api/Analise", {
            method: "POST",
            headers: { "Content-Type": "application/json", "Authorization": `Bearer ${token}` },
            body: JSON.stringify(analiseParaSalvar),
        });
        if (!response.ok) {
            if (response.status === 401) { alert("Sessão expirou. Faça login novamente."); logout(); return; }
            throw new Error("Ocorreu um erro ao salvar a análise.");
        }
        window.location.href = '/salvos';
    } catch (error) {
        alert(`Falha ao salvar: ${error.message}`);
    } finally {
        saveButton.disabled = false;
        saveButton.innerHTML = originalButtonContent;
    }
}

function exportCSV() {
    if (!window.chartData.datasets.length) { alert('Nenhum dado para exportar!'); return; }
    
    const headers = [];
    window.chartData.datasets.forEach(d => {
        headers.push(`${d.label} (${window.rotuloX})`, `${d.label} (${window.rotuloY})`);
    });
    let csvContent = `data:text/csv;charset=utf-8,${headers.join(';')}\n`;

    const maxRows = Math.max(...window.chartData.datasets.map(d => d.data.length));

    for (let i = 0; i < maxRows; i++) {
        const row = [];
        window.chartData.datasets.forEach(dataset => {
            if (i < dataset.data.length) {
                row.push(dataset.data[i].x, dataset.data[i].y);
            } else {
                row.push('', '');
            }
        });
        csvContent += `${row.join(';')}\n`;
    }

    const link = document.createElement("a");
    link.setAttribute("href", encodeURI(csvContent));
    link.setAttribute("download", "analise_uvvis_completa.csv");
    link.click();
}