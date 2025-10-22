// Armazena as análises carregadas da API
let loadedAnalyses = [];
// Mapeia IDs de canvas para instâncias de gráfico
let activeCharts = {};

async function loadSavedAnalyses() {
    Object.values(activeCharts).forEach(chart => chart.destroy());
    activeCharts = {};

    const container = document.getElementById('savedAnalysesContainer');
    const token = localStorage.getItem('token');

    if (!token) {
        container.innerHTML = '<h2>Análises Salvas</h2><p>Você precisa estar logado para ver suas análises. Por favor, <a href="#" onclick="openAuthModal(\'login\')">faça o login</a>.</p>';
        return;
    }

    container.innerHTML = '<h2>Carregando análises...</h2>';

    try {
        const response = await fetch("https://localhost:5001/api/Analise/GetAnalisesByUsuario", {
            method: 'GET',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) {
            if (response.status === 401) {
                container.innerHTML = '<h2>Sessão Expirada</h2><p>Sua sessão expirou. Por favor, <a href="#" onclick="openAuthModal(\'login\')">faça o login</a> novamente.</p>';
                logout();
            } else {
                throw new Error('Falha ao carregar as análises.');
            }
            return;
        }

        const analysesFromApi = await response.json();
        loadedAnalyses = analysesFromApi;

        container.innerHTML = '<h2>Análises Salvas</h2>';

        if (!loadedAnalyses.length) {
            container.innerHTML += '<p>Nenhuma análise salva ainda.</p>';
            return;
        }

        loadedAnalyses.forEach((analysis, index) => {
            const card = document.createElement('div');
            card.className = 'graph-card';
            const uniqueId = analysis.id !== undefined ? analysis.id : index;
            const canvasId = `chart-${uniqueId}`;

            const infoHTML = `
                <div class="analysis-info">
                    <h3>${analysis.nome}</h3>
                    <p><strong>Descrição:</strong> ${analysis.descricao || 'Nenhuma descrição fornecida.'}</p>
                </div>
            `;

            const btnsHTML = `
                <div class="btn-container">
                    <button class="btn btn-outline" onclick="deleteAnalysis(${analysis.id}, this)">
                        <i class="fas fa-trash"></i> Excluir
                    </button>
                    <button class="btn btn-outline" onclick="downloadGraph('${canvasId}', '${analysis.nome}')">
                        <i class="fas fa-download"></i> Baixar PNG
                    </button>
                    <button class="btn btn-outline" onclick="exportAnalysisCSV(${analysis.id}, '${analysis.nome}')">
                        <i class="fas fa-file-csv"></i> Exportar CSV
                    </button>
                    <button class="btn btn-outline" id="toggle-picos-${canvasId}" onclick="toggleSavedChartPicos('${canvasId}')">
                        <i class="fas fa-search-minus"></i> Ocultar Bandas
                    </button>
                </div>
            `;

            card.innerHTML = `
                ${infoHTML}
                ${btnsHTML}
                <div class="graph-img-container" style="height: 450px;">
                    <canvas id="${canvasId}"></canvas>
                </div>
            `;
            
            container.appendChild(card);
            renderSavedChart(canvasId, analysis);
        });

    } catch (error) {
        console.error("Erro ao carregar análises:", error);
        container.innerHTML = '<h2>Erro</h2><p>Não foi possível carregar suas análises. Tente novamente mais tarde.</p>';
    }
}

function renderSavedChart(canvasId, analysis) {
    const canvas = document.getElementById(canvasId);
    if (!canvas || !analysis.linhas || analysis.linhas.length === 0) return;

    const ctx = canvas.getContext('2d');
    const lineColors = ['#2b2be8', '#e82b2b', '#2be82b', '#e89c2b', '#9c2be8', '#2be8e8'];
    const finalDatasets = [];
    let verticalOffset = 0;

    console.log("Linhas: ", analysis.linhas);

    analysis.linhas.forEach((linha, index) => {
        const yValues = linha.dados.map(d => d.valorY);
        if (yValues.length === 0) return;

        const localMinY = Math.min(...yValues);
        const localMaxY = Math.max(...yValues);
        const shiftAmount = verticalOffset - localMinY;

        const transformedData = linha.dados.map(d => ({ x: d.valorX, y: d.valorY + shiftAmount }));
        const color = linha.cor || lineColors[index % lineColors.length];

        console.log("Linha: ", linha.cor);

        finalDatasets.push({
            label: linha.nome,
            data: transformedData,
            borderColor: color,
            borderWidth: 2,
            fill: false,
            tension: 0.1,
            pointRadius: 0
        });

        const maxValor = Math.max(...yValues);
        const maxIndex = yValues.indexOf(maxValor);
        const picoData = [{ x: linha.dados[maxIndex].valorX, y: maxValor + shiftAmount }];

        finalDatasets.push({
            label: `Banda (${linha.nome})`,
            data: picoData,
            backgroundColor: 'black',
            pointRadius: 6,
            showLine: false
        });

        const range = localMaxY - localMinY;
        verticalOffset += range + (range * 0.05);
    });

    const newChart = new Chart(ctx, {
        type: 'line',
        data: { datasets: finalDatasets },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    callbacks: {
                        label: function(tooltipItem) {
                            if (tooltipItem.dataset.label.startsWith('Banda')) {
                                return null;
                            }
                            const originalDatasetIndex = Math.floor(tooltipItem.datasetIndex / 2);
                            const originalPoint = analysis.linhas[originalDatasetIndex].dados[tooltipItem.dataIndex];
                            if (originalPoint) {
                                return `${tooltipItem.dataset.label}: ${originalPoint.valorY.toFixed(4)}`;
                            }
                            return '';
                        }
                    }
                }
            },
            scales: {
                x: { type: 'linear', title: { display: true, text: analysis.rotuloX }, reverse: true, ticks: { maxTicksLimit: 40 } },
                y: {
                    title: { display: true, text: analysis.rotuloY },
                    ticks: {
                        display: false
                    }
                }
            }
        }
    });
    activeCharts[canvasId] = newChart;
}

function toggleSavedChartPicos(canvasId) {
    const chart = activeCharts[canvasId];
    const button = document.getElementById(`toggle-picos-${canvasId}`);
    if (!chart || !button) return;

    const firstBandaDataset = chart.data.datasets.find(d => d.label.startsWith('Banda'));
    if (!firstBandaDataset) return;

    // Verifica se as bandas estão atualmente visíveis (propriedade hidden é false ou undefined)
    const areBandsVisible = !firstBandaDataset.hidden;

    // O novo estado 'hidden' será o oposto da visibilidade atual.
    // Se estavam visíveis, newHiddenState será true (para esconder).
    const newHiddenState = areBandsVisible;

    chart.data.datasets.forEach(dataset => {
        if (dataset.label.startsWith('Banda')) {
            dataset.hidden = newHiddenState;
        }
    });

    // Atualiza o texto do botão. Se as bandas agora estão escondidas (newHiddenState = true),
    // o botão deve oferecer a opção de "Mostrar".
    button.innerHTML = newHiddenState
        ? `<i class="fas fa-search-plus"></i> Mostrar Bandas`
        : `<i class="fas fa-search-minus"></i> Ocultar Bandas`;
        
    chart.update();
}

async function deleteAnalysis(id, buttonElement) {
    if (typeof id !== 'number' || id === undefined) {
        alert("Não foi possível excluir a análise porque seu identificador é inválido.");
        return;
    }
    if (!confirm('Tem certeza que deseja excluir esta análise?')) return;

    const originalButtonContent = buttonElement.innerHTML;
    buttonElement.disabled = true;
    buttonElement.innerHTML = `Excluindo <span class="loading-dots"></span>`;

    const token = localStorage.getItem('token');
    try {
        const response = await fetch(`https://localhost:5001/api/Analise/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (!response.ok) throw new Error('Falha ao excluir a análise.');
        alert('Análise excluída com sucesso!');
        loadSavedAnalyses();
    } catch (error) {
        console.error('Erro ao excluir:', error);
        alert(`Não foi possível excluir a análise: ${error.message}`);
        buttonElement.disabled = false;
        buttonElement.innerHTML = originalButtonContent;
    }
}

function downloadGraph(canvasId, name) {
    const canvas = document.getElementById(canvasId);
    const link = document.createElement('a');
    link.href = canvas.toDataURL('image/png');
    link.download = `${name}.png`;
    link.click();
}

function exportAnalysisCSV(analysisId, name) {
    const analysis = loadedAnalyses.find(a => a.id === analysisId);
    if (!analysis || !analysis.linhas || analysis.linhas.length === 0) return;
    const headers = [];
    analysis.linhas.forEach(l => {
        headers.push(`${l.nome} (${analysis.rotuloX})`, `${l.nome} (${analysis.rotuloY})`);
    });
    let csvContent = `data:text/csv;charset=utf-8,${headers.join(';')}\n`;
    const maxRows = Math.max(...analysis.linhas.map(l => l.dados.length));
    for (let i = 0; i < maxRows; i++) {
        const row = [];
        analysis.linhas.forEach(linha => {
            if (i < linha.dados.length) {
                row.push(linha.dados[i].valorX, linha.dados[i].valorY);
            } else {
                row.push('', '');
            }
        });
        csvContent += `${row.join(';')}\n`;
    }
    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", `${name}_valores.csv`);
    link.click();
}

window.addEventListener('DOMContentLoaded', loadSavedAnalyses);