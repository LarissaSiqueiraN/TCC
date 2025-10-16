function loadSavedAnalyses() {
    const container = document.getElementById('savedAnalysesContainer');
    container.innerHTML = '<h2>Análises Salvas</h2>';

    const savedAnalyses = JSON.parse(localStorage.getItem('savedAnalyses') || '[]');

    if (!savedAnalyses.length) {
        container.innerHTML += '<p>Nenhuma análise salva ainda.</p>';
        return;
    }

    savedAnalyses.forEach(analysis => {
        const card = document.createElement('div');
        card.className = 'graph-card';

        // Botões principais
        const btnsHTML = `
            <div class="btn-container">
                <button class="btn btn-outline" onclick="deleteAnalysis(${analysis.id})">
                    <i class="fas fa-trash"></i> Excluir
                </button>
                <button class="btn btn-outline" onclick="downloadGraph('${analysis.image}', '${analysis.name}')">
                    <i class="fas fa-download"></i> Baixar PNG
                </button>
                <button class="btn btn-outline" onclick="exportAnalysisCSV(${analysis.id}, '${analysis.name}')">
                    <i class="fas fa-file-csv"></i> Exportar CSV
                </button>
            </div>
        `;

        // Informações da análise
        const infoHTML = `
            <div class="analysis-info">
                <h3>${analysis.name}</h3>
                <p><strong>Informações:</strong> ${analysis.description}</p>
            </div>
        `;

        // Criar tabela
        const table = document.createElement('table');
        table.className = 'data-table';
        let tbodyHTML = `<thead>
                            <tr>
                                <th>
                                  Wavelength (nm)
                                  <button class="btn-copy" onclick="copyColumn(${analysis.id}, 0)">📋</button>
                                </th>
                                <th>
                                  Absorbância (Abs)
                                  <button class="btn-copy" onclick="copyColumn(${analysis.id}, 1)">📋</button>
                                </th>
                            </tr>
                        </thead>
                        <tbody>`;
        for (let i = 0; i < analysis.labels.length; i++) {
            tbodyHTML += `<tr>
                            <td>${analysis.labels[i]}</td>
                            <td>${analysis.data[i]}</td>
                          </tr>`;
        }
        tbodyHTML += `</tbody>`;
        table.innerHTML = tbodyHTML;

        // Botão para mostrar/ocultar tabela
        const toggleTableBtn = document.createElement('button');
        toggleTableBtn.innerText = 'Mostrar/Ocultar Tabela';
        toggleTableBtn.className = 'btn btn-outline';
        toggleTableBtn.style.marginBottom = '10px';
        toggleTableBtn.addEventListener('click', () => {
            table.style.display = table.style.display === 'none' ? 'table' : 'none';
        });

        // Adicionar tudo ao card
        card.innerHTML = `
            ${infoHTML}
            ${btnsHTML}
            <img class="graph-img" src="${analysis.image}" alt="${analysis.name}">
        `;
        card.appendChild(toggleTableBtn);
        card.appendChild(table);

        // Separador
        const separator = document.createElement('div');
        separator.className = 'graph-separator';
        card.appendChild(separator);

        container.appendChild(card);
    });
}

function deleteAnalysis(id) {
    let savedAnalyses = JSON.parse(localStorage.getItem('savedAnalyses') || '[]');
    savedAnalyses = savedAnalyses.filter(a => a.id !== id);
    localStorage.setItem('savedAnalyses', JSON.stringify(savedAnalyses));
    loadSavedAnalyses();
}

function downloadGraph(imageBase64, name) {
    const link = document.createElement('a');
    link.href = imageBase64;
    link.download = `${name}.png`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// Função para copiar coluna da tabela
function copyColumn(analysisId, colIndex) {
    const savedAnalyses = JSON.parse(localStorage.getItem('savedAnalyses') || '[]');
    const analysis = savedAnalyses.find(a => a.id === analysisId);
    if (!analysis) return;

    let values = [];
    if (colIndex === 0) values = analysis.labels;
    else if (colIndex === 1) values = analysis.data;

    // Copiando separado por ponto e vírgula
    const textToCopy = values.join(';');
    navigator.clipboard.writeText(textToCopy)
        .then(() => alert('Coluna copiada para o clipboard!'))
        .catch(err => alert('Erro ao copiar: ' + err));
}

// Função para exportar CSV completo do gráfico
function exportAnalysisCSV(analysisId, name) {
    const savedAnalyses = JSON.parse(localStorage.getItem('savedAnalyses') || '[]');
    const analysis = savedAnalyses.find(a => a.id === analysisId);
    if (!analysis) return;

    let csvContent = "data:text/csv;charset=utf-8,";
    for (let i = 0; i < analysis.labels.length; i++) {
        csvContent += `${analysis.labels[i]};${analysis.data[i]}\n`;
    }

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", `${name}_valores.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}


window.addEventListener('DOMContentLoaded', loadSavedAnalyses);
