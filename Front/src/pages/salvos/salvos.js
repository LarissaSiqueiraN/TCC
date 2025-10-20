// Armazena as análises carregadas da API
let loadedAnalyses = [];
// **Alterado para um objeto para mapear IDs de canvas para instâncias de gráfico**
let activeCharts = {};

async function loadSavedAnalyses() {
    // Destrói os gráficos antigos usando os valores do objeto
    Object.values(activeCharts).forEach(chart => chart.destroy());
    activeCharts = {}; // Limpa o objeto

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
                    <button class="btn btn-outline" onclick="deleteAnalysis(${analysis.id})">
                        <i class="fas fa-trash"></i> Excluir
                    </button>
                    <button class="btn btn-outline" onclick="downloadGraph('${canvasId}', '${analysis.nome}')">
                        <i class="fas fa-download"></i> Baixar PNG
                    </button>
                    <button class="btn btn-outline" onclick="exportAnalysisCSV(${analysis.id}, '${analysis.nome}')">
                        <i class="fas fa-file-csv"></i> Exportar CSV
                    </button>
                    <!-- BOTÃO MOSTRAR PICOS ADICIONADO -->
                    <button class="btn btn-outline" id="toggle-picos-${canvasId}" onclick="toggleSavedChartPicos('${canvasId}')">
                        <i class="fas fa-search-plus"></i> Mostrar Picos
                    </button>
                </div>
            `;

            card.innerHTML = `
                ${infoHTML}
                ${btnsHTML}
                <div class="graph-img-container">
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
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    
    const labels = analysis.dados.map(d => d.valorX);
    const data = analysis.dados.map(d => d.valorY);

    const maxValor = Math.max(...data);
    const maxIndex = data.indexOf(maxValor);
    const picoData = new Array(data.length).fill(null);
    picoData[maxIndex] = maxValor;

    const newChart = new Chart(ctx, {
        type: analysis.tipo || 'line',
        data: {
            labels: labels,
            datasets: [{
                label: analysis.rotuloY,
                data: data,
                borderColor: 'rgba(43, 43, 232, 1)',
                backgroundColor: 'transparent',
                borderWidth: 2,
                fill: false,
                tension: 0.1,
                pointRadius: 0
            }, {
                label: 'Pico mais alto', 
                data: picoData,
                backgroundColor: 'black',
                borderColor: 'black',
                pointRadius: 6,
                pointHoverRadius: 8,
                showLine: false, 
                type: 'line'
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: { 
                    type: 'linear',
                    title: { display: true, text: analysis.rotuloX },
                    reverse: true,
                    offset: true,
                    ticks: { autoSkip: false, maxTicksLimit: 40  }
                },
                y: { 
                    title: { display: true, text: analysis.rotuloY },
                    beginAtZero: true,
                    suggestedMax: Math.max(...data) * 1.1
                 }
            }
        }
    });

    // Armazena a instância do gráfico no objeto usando o ID do canvas como chave
    activeCharts[canvasId] = newChart;
}

// **NOVA FUNÇÃO PARA ALTERNAR OS PICOS**
function toggleSavedChartPicos(canvasId) {
    const chart = activeCharts[canvasId];
    const button = document.getElementById(`toggle-picos-${canvasId}`);

    if (!chart || !button) return;

    // Verifica o estado atual pelo raio do ponto do primeiro dataset
    const dataset = chart.data.datasets[0];
    const picosEstaoVisiveis = dataset.pointRadius > 0;

    if (picosEstaoVisiveis) {
        // Esconde os picos
        dataset.pointRadius = 0;
        button.innerHTML = `<i class="fas fa-search-plus"></i> Mostrar Picos`;
    } else {
        // Mostra os picos
        dataset.pointRadius = 3;
        dataset.pointBackgroundColor = 'rgba(43, 43, 232, 1)';
        button.innerHTML = `<i class="fas fa-search-minus"></i> Ocultar Picos`;
    }

    // Atualiza o gráfico para aplicar as mudanças
    chart.update();
}

async function deleteAnalysis(id) {
    if (!confirm('Tem certeza que deseja excluir esta análise?')) return;

    const token = localStorage.getItem('token');
    try {
        const response = await fetch(`https://localhost:5001/api/Analise/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Falha ao excluir.');

        alert('Análise excluída com sucesso!');
        loadSavedAnalyses();
    } catch (error) {
        console.error('Erro ao excluir:', error);
        alert('Não foi possível excluir a análise.');
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
    if (!analysis) return;

    let csvContent = `data:text/csv;charset=utf-8,${analysis.rotuloX};${analysis.rotuloY}\n`;
    analysis.dados.forEach(d => {
        csvContent += `${d.valorX};${d.valorY}\n`;
    });

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", `${name}_valores.csv`);
    link.click();
}

window.addEventListener('DOMContentLoaded', loadSavedAnalyses);