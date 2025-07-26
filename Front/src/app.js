const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 3000;

const server = http.createServer((req, res) => {
    let filePath;

    if (req.url === '/' || req.url === '/inicio') {
        filePath = path.join(__dirname, 'pages', 'inicio', 'inicio.html');
    } else if (req.url.startsWith('/pages/')) {
        filePath = path.join(__dirname, 'pages', req.url.replace('/inicio/', ''));
    } else if (req.url.startsWith('/components/')) {
        filePath = path.join(__dirname, 'components', req.url.replace('/components/', ''));
    } 
    else if (req.url.startsWith('/publics/')) {
        filePath = path.join(__dirname, 'publics', req.url.replace('/publics/', ''));
    }
    else {
        filePath = path.join(__dirname, 'pages', req.url);
    }

    const extname = String(path.extname(filePath)).toLowerCase();
    const mimeTypes = {
        '.html': 'text/html',
        '.js': 'text/javascript',
        '.css': 'text/css',
        '.png': 'image/png'
    };

    const contentType = mimeTypes[extname] || 'application/octet-stream';

    fs.readFile(filePath, (error, content) => {
        if (error) {
            res.writeHead(404, { 'Content-Type': 'text/plain' });
            res.end('Pagina nao encontrada');
        } else {
            res.writeHead(200, { 'Content-Type': contentType });
            res.end(content, 'utf-8');
        }
    });
});

server.listen(PORT, () => {
    console.log(`Servidor rodando em http://localhost:${PORT}`);
});