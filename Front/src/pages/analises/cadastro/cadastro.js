// Variáveis globais
window.uvVisChart = null;
window.currentLabels = [];
window.currentData = [];
// Variáveis para armazenar os rótulos dos eixos, com valores padrão
window.rotuloX = "Comprimento de Onda (nm)";
window.rotuloY = "Absorbância (Abs)";

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


    const maxValor = Math.max(...window.currentData);
    const maxIndex = window.currentData.indexOf(maxValor);

  
    const picoData = new Array(window.currentData.length).fill(null);
    picoData[maxIndex] = maxValor;


    placeholder.style.display = 'none';
    canvas.style.display = 'block';

    const ctx = canvas.getContext('2d');

    if (window.uvVisChart) window.uvVisChart.destroy();

    window.uvVisChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: window.currentLabels.map(x => parseFloat(x).toFixed(1)),
            datasets: [{
                label: window.rotuloY, 
                data: window.currentData,
                borderColor: 'rgba(43, 43, 232, 1)',
                backgroundColor: 'transparent',
                borderWidth: 2,
                fill: false,
                tension: 0.1,
                pointRadius: 0
            },
            {
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
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'linear',
                    title: { display: true, text: window.rotuloX }, 
                    reverse: true,
                    offset: true,
                    ticks: { autoSkip: false, maxTicksLimit: 40  }
                },
                y: {
                    title: { display: true, text: window.rotuloY },
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
    // Reseta os rótulos para os valores padrão
    window.rotuloX = "Comprimento de Onda (nm)";
    window.rotuloY = "Absorbância (Abs)";

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

    // Usa os rótulos capturados no cabeçalho do CSV
    let csvContent = `data:text/csv;charset=utf-8,${window.rotuloX},${window.rotuloY}\n`;
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


    btnCarregar.addEventListener('click', function(e) {
        e.preventDefault();
        fileInput.click();
    });


    fileInput.addEventListener('change', function(event) {
        const file = event.target.files[0];
        if (!file) return;

        const fileName = file.name.toLowerCase();
        // Limpa os dados antes de carregar um novo arquivo
        // resetAnalysis();

        if (fileName.endsWith('.csv')) {
            Papa.parse(file, {
                header: false,
                skipEmptyLines: true,
                delimiter: ";",
                complete: function(results) {
                    if (results.data.length < 2) {
                        alert("O arquivo CSV precisa ter um cabeçalho e pelo menos uma linha de dados.");
                        return;
                    }
                    // **Captura os rótulos da primeira linha**
                    window.rotuloX = results.data[0][0] || "Eixo X";
                    window.rotuloY = results.data[0][1] || "Eixo Y";

                    // Pega os dados a partir da segunda linha
                    const dataRows = results.data.slice(1);
                    dataRows.forEach(row => {
                        window.currentLabels.push(String(row[0]).replace(',', '.'));
                        window.currentData.push(String(row[1]).replace(',', '.'));
                    });

                    // Validação de números
                    const invalidData = window.currentLabels.some((x,i) =>
                        isNaN(parseFloat(x)) || isNaN(parseFloat(window.currentData[i]))
                    );
                    if (invalidData) {
                        alert('O arquivo contém valores inválidos! Todos os valores devem ser números.');
                        resetAnalysis();
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

                    if (excelData.length < 2) {
                        alert("O arquivo Excel precisa ter um cabeçalho e pelo menos uma linha de dados.");
                        return;
                    }

                    // **Captura os rótulos da primeira linha**
                    window.rotuloX = excelData[0][0] || "Eixo X";
                    window.rotuloY = excelData[0][1] || "Eixo Y";

                    // Pega os dados a partir da segunda linha
                    for (let i = 1; i < excelData.length; i++) {
                        if (excelData[i] && excelData[i][0] !== undefined && excelData[i][1] !== undefined) {
                            window.currentLabels.push(excelData[i][0]);
                            window.currentData.push(excelData[i][1]);
                        }
                    }

                    // Validação de números
                    const invalidData = window.currentLabels.some((x,i) =>
                        isNaN(parseFloat(x)) || isNaN(parseFloat(window.currentData[i]))
                    );
                    if (invalidData) {
                        alert('O arquivo contém valores inválidos! Todos os valores devem ser números.');
                        resetAnalysis();
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
        
        // Usa os rótulos padrão para entrada manual
        window.rotuloX = "Comprimento de Onda (nm)";
        window.rotuloY = "Absorbância (Abs)";

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

    if (picosVisiveis) {
        togglePointsBtn.innerHTML = '<i class="fas fa-search-minus"></i> Ocultar Picos';
    } else {
        togglePointsBtn.innerHTML = '<i class="fas fa-search-plus"></i> Mostrar Picos';
    }
    
    window.uvVisChart.update();
});

async function saveAnalysis() {
    const saveButton = document.getElementById('btnSave');
    const originalButtonContent = saveButton ? saveButton.innerHTML : '';

    if (saveButton) {
        saveButton.disabled = true;
        saveButton.innerHTML = `Salvando <span class="loading-dots"></span>`;
    }


    const token = localStorage.getItem('token');
    
    console.log("Token recuperado:", token);

    if (!token) {
        alert('Você precisa estar logado para salvar análises.');
        return;
    }
    
    const nome = document.getElementById('project-name').value.trim();
    const descricao = document.getElementById('project-description').value.trim();
  
    if (!window.currentLabels || window.currentLabels.length === 0) {
        alert("Não há dados carregados no gráfico para salvar.");
        return;
    }

    const dados = window.currentLabels.map((valorX, index) => ({
        ValorX: valorX,
        ValorY: window.currentData[index],
    }));

    const analiseParaSalvar = {
        Nome: nome,
        Descricao: descricao,
        // **Usa os rótulos capturados do arquivo**
        RotuloX: window.rotuloX,
        RotuloY: window.rotuloY,
        Dados: dados,
    };

 
  console.log("Enviando para a API:", analiseParaSalvar);

  // 6. Enviar a requisição para a API
  try {
    const response = await fetch("http://localhost:5000/api/Analise", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
      body: JSON.stringify(analiseParaSalvar),
    });

    if (!response.ok) {
      if (response.status === 401) {
        alert("Sua sessão expirou. Por favor, faça o login novamente.");
        logout(); 
        return; 
      }
      
      const errorData = await response.json();
      throw new Error(errorData.message || "Ocorreu um erro ao salvar a análise.");
    }

    alert("Análise salva com sucesso!");
    window.location.href = '/salvos';
    resetAnalysis();

  } catch (error) {
    console.error("Erro ao salvar a análise:", error);
    alert(`Falha ao salvar: ${error.message}`);
  }
  finally {
        if (saveButton) {
            saveButton.disabled = false;
            saveButton.innerHTML = originalButtonContent;
        }
    }
}