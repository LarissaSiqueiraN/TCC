// Variáveis globais
window.uvVisChart = null;
window.currentLabels = [];
window.currentData = [];

// Tipo de gráfico fixo
window.tipoGrafico = 'line';

// Função para gerar gráfico
function gerarGrafico() {
    const canvas = document.getElementById('uvVisChart');
    const placeholder = document.getElementById('placeholder');

    if (!window.currentLabels.length || !window.currentData.length) {
        alert('Nenhum dado carregado!');
        return;
    }

    placeholder.style.display = 'none';
    canvas.style.display = 'block';

    const ctx = canvas.getContext('2d');

    if (window.uvVisChart) window.uvVisChart.destroy();

    window.uvVisChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: window.currentLabels.map(x => parseFloat(x).toFixed(1)),
            datasets: [{
                label: 'Absorbância',
                data: window.currentData,
                borderColor: 'rgba(43, 43, 232, 1)',
                backgroundColor: 'transparent',
                borderWidth: 2,
                fill: false,
                tension: 0.1,
                pointRadius: 0
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'linear',
                    title: { display: true, text: 'Wavelength (nm)' },
                    reverse: true,
                    offset: true,
                    ticks: { autoSkip: false }
                },
                y: {
                    title: { display: true, text: 'Absorbance (Abs)' },
                    beginAtZero: true,
                    suggestedMax: Math.max(...window.currentData) * 1.1
                }
            }
        }
    });
}

// Redefinir análise
function resetAnalysis() {
    document.getElementById('project-name').value = '';
    document.getElementById('project-description').value = '';
    document.getElementById('nm-input').value = '';
    document.getElementById('abs-input').value = '';
    document.getElementById('fileInput').value = '';

    window.currentLabels = [];
    window.currentData = [];

    if (window.uvVisChart) {
        window.uvVisChart.destroy();
        window.uvVisChart = null;
    }

    document.getElementById('uvVisChart').style.display = 'none';
    document.getElementById('placeholder').style.display = 'block';
}

// Exportar CSV
function exportCSV() {
    if (!window.currentLabels.length || !window.currentData.length) {
        alert('Nenhum dado para exportar!');
        return;
    }

    let csvContent = "data:text/csv;charset=utf-8,Wavelength (nm),Absorbance (Abs)\n";
    for (let i = 0; i < window.currentLabels.length; i++) {
        csvContent += `${window.currentLabels[i]},${window.currentData[i]}\n`;
    }

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "analise_uvvis.csv");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// DOMContentLoaded
window.addEventListener('DOMContentLoaded', function() {
    const btnCarregar = document.getElementById('btnUpload');
    const fileInput = document.getElementById('fileInput');
    const nmInput = document.getElementById('nm-input');
    const absInput = document.getElementById('abs-input');
    const btnConfig = document.getElementById('btnConfig');
    const btnExport = document.getElementById('btnExport');

    // Abrir explorador de arquivos
    btnCarregar.addEventListener('click', function(e) {
        e.preventDefault();
        fileInput.click();
    });

    // Ler arquivos CSV ou Excel
    fileInput.addEventListener('change', function(event) {
        const file = event.target.files[0];
        if (!file) return;

        const fileName = file.name.toLowerCase();
        window.currentLabels = [];
        window.currentData = [];

        if (fileName.endsWith('.csv')) {
            Papa.parse(file, {
                header: false,
                skipEmptyLines: true,
                delimiter: ";",
                complete: function(results) {
                    results.data.forEach(row => {
                        window.currentLabels.push(row[0].replace(',', '.'));
                        window.currentData.push(row[1]);
                    });

                    // Validação de números
                    const invalidData = window.currentLabels.some((x,i) =>
                        isNaN(parseFloat(x)) || isNaN(parseFloat(window.currentData[i]))
                    );
                    if (invalidData) {
                        alert('O arquivo contém valores inválidos! Todos os valores devem ser números.');
                        window.currentLabels = [];
                        window.currentData = [];
                        return;
                    }

                    // Transformar em números
                    window.currentLabels = window.currentLabels.map(x => parseFloat(x));
                    window.currentData = window.currentData.map(x => parseFloat(x));

                    gerarGrafico();
                },
                error: function(err) { console.error("Erro ao ler CSV:", err); }
            });

        } else if (fileName.endsWith('.xlsx') || fileName.endsWith('.xls')) {
            const reader = new FileReader();
            reader.onload = function(e) {
                try {
                    const data = new Uint8Array(e.target.result);
                    const workbook = XLSX.read(data, { type: 'array' });
                    const sheet = workbook.Sheets[workbook.SheetNames[0]];
                    const excelData = XLSX.utils.sheet_to_json(sheet, { header: 1 });

                    for (let i = 1; i < excelData.length; i++) {
                        window.currentLabels.push(excelData[i][0]);
                        window.currentData.push(excelData[i][1]);
                    }

                    // Validação de números
                    const invalidData = window.currentLabels.some((x,i) =>
                        isNaN(parseFloat(x)) || isNaN(parseFloat(window.currentData[i]))
                    );
                    if (invalidData) {
                        alert('O arquivo contém valores inválidos! Todos os valores devem ser números.');
                        window.currentLabels = [];
                        window.currentData = [];
                        return;
                    }

                    // Transformar em números
                    window.currentLabels = window.currentLabels.map(x => parseFloat(x));
                    window.currentData = window.currentData.map(x => parseFloat(x));

                    gerarGrafico();

                } catch (err) { console.error("Erro ao ler Excel:", err); }
            };
            reader.readAsArrayBuffer(file);
        } else {
            alert('Formato de arquivo não suportado! Selecione CSV ou Excel.');
        }
    });

    // Carregar dados manualmente
    const btnContainer = document.createElement('div');
    btnContainer.style.textAlign = 'center';
    btnContainer.style.marginTop = '40px';

    const btnManual = document.createElement('button');
    btnManual.id = 'btnManual';
    btnManual.innerText = 'Carregar Dados Manualmente';
    btnManual.className = 'btn btn-outline';
    btnManual.style.display = 'inline-block';

    btnContainer.appendChild(btnManual);
    nmInput.parentElement.parentElement.appendChild(btnContainer);

    btnManual.addEventListener('click', () => {
        const nmArray = nmInput.value.split(';').map(x => x.trim());
        const absArray = absInput.value.split(';').map(x => x.trim());

        if (nmArray.length !== absArray.length) {
            alert('O número de valores de nm e Abs não coincide!');
            return;
        }

        // Verificar se todos os valores são números
        const nmInvalid = nmArray.some(x => isNaN(parseFloat(x)));
        const absInvalid = absArray.some(x => isNaN(parseFloat(x)));

        if (nmInvalid || absInvalid) {
            alert('Todos os valores devem ser números válidos!');
            return;
        }

        // Transformar em números e gerar gráfico
        window.currentLabels = nmArray.map(x => parseFloat(x));
        window.currentData = absArray.map(x => parseFloat(x));

        gerarGrafico();
    });

    // Botão exportar
    if (btnExport) btnExport.addEventListener('click', exportCSV);
});

// Toggle picos
const togglePointsBtn = document.getElementById('togglePointsBtn');
let picosVisiveis = false;

togglePointsBtn.addEventListener('click', () => {
    if (!window.uvVisChart) return;

    picosVisiveis = !picosVisiveis;

    window.uvVisChart.data.datasets[0].pointRadius = picosVisiveis ? 3 : 0;
    window.uvVisChart.data.datasets[0].pointBackgroundColor = picosVisiveis
        ? 'rgba(43, 43, 232, 1)'
        : 'rgba(43, 43, 232, 0)';

    togglePointsBtn.innerText = picosVisiveis ? 'Ocultar Picos' : 'Mostrar Picos';

    window.uvVisChart.update();
});

function saveAnalysis() {
    const name = document.getElementById('project-name').value.trim();
    const description = document.getElementById('project-description').value.trim();

    if (!name || !window.currentLabels.length || !window.currentData.length) {
        alert('Preencha o nome e carregue os dados do gráfico antes de salvar.');
        return;
    }

    // Pega a imagem do gráfico
    const canvas = document.getElementById('uvVisChart');
    const imageBase64 = canvas.toDataURL('image/png'); // imagem do gráfico

    const analysis = {
        id: Date.now(), // ID único
        name,
        description,
        labels: window.currentLabels,
        data: window.currentData,
        image: imageBase64,
        createdAt: new Date().toISOString()
    };

    // Pega o array de análises salvas no localStorage
    let savedAnalyses = JSON.parse(localStorage.getItem('savedAnalyses') || '[]');

    // Adiciona a nova análise
    savedAnalyses.push(analysis);

    // Salva novamente
    localStorage.setItem('savedAnalyses', JSON.stringify(savedAnalyses));

    alert('Análise salva com sucesso! Agora você pode visualizá-la na página de Salvos.');
}
