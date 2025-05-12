// Initialize PDF.js
let pdfDoc = null;
let pageNum = 1;
let pageRendering = false;
let pageNumPending = null;
let scale = 1.5;
let dotNetHelper = null;

async function initPdfViewer(helper, pdfUrl, initialPage = 1) {
    dotNetHelper = helper;

    // Load PDF.js if not already loaded
    if (!window.pdfjsLib) {
        await loadScript('https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.min.js');
        // Set workerSrc property
        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.worker.min.js';
    }

    // Load the PDF
    try {
        pdfDoc = await window.pdfjsLib.getDocument(pdfUrl).promise;
        pageNum = initialPage || 1;

        // Update page count
        dotNetHelper.invokeMethodAsync('UpdatePageInfo', pageNum, pdfDoc.numPages);

        // Initial render
        renderPage(pageNum);
    } catch (error) {
        console.error('Error loading PDF:', error);
    }
}

function renderPage(num) {
    pageRendering = true;

    // Get the page
    pdfDoc.getPage(num).then(function (page) {
        const viewport = page.getViewport({ scale });
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        const container = document.getElementById('pdfViewer');

        // Clear previous content
        container.innerHTML = '';
        container.appendChild(canvas);

        // Set dimensions
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page
        const renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };

        const renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRendering = false;

            // Update .NET component
            dotNetHelper.invokeMethodAsync('UpdatePageInfo', num, pdfDoc.numPages);

            if (pageNumPending !== null) {
                // New page rendering is pending
                renderPage(pageNumPending);
                pageNumPending = null;
            }
        });
    });
}

function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}

function goToPage(num) {
    if (pdfDoc === null || num < 1 || num > pdfDoc.numPages) return;

    pageNum = num;
    queueRenderPage(num);
}

// Helper function to load scripts dynamically
function loadScript(src) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = src;
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
}

// Annotation sidebar functionality
window.annotationSidebar = {
    initialize: function () {
        const toggleButton = document.querySelector('.toggle-annotations');
        const sidebar = document.querySelector('.annotation-sidebar');

        if (toggleButton && sidebar) {
            // Check on load
            this.checkWidth();
            // Add resize listener
            window.addEventListener('resize', this.checkWidth);
        }
    },

    checkWidth: function () {
        const sidebar = document.querySelector('.annotation-sidebar');
        if (sidebar) {
            if (window.innerWidth < 992) {
                sidebar.style.display = 'none';
            }
        }
    },

    toggle: function (show) {
        const sidebar = document.querySelector('.annotation-sidebar');
        const toggleButton = document.querySelector('.toggle-annotations');

        if (sidebar && toggleButton) {
            if (show) {
                sidebar.style.display = 'block';
                toggleButton.textContent = 'Hide Annotations';
            } else {
                sidebar.style.display = 'none';
                toggleButton.textContent = 'Show Annotations';
            }
        }
    }
};

// Admin tabs functionality
window.adminTabs = {
    initialize: function () {
        const tabs = document.querySelectorAll('.tab');

        if (tabs.length > 0) {
            tabs.forEach(tab => {
                tab.addEventListener('click', function () {
                    // Remove active class from all tabs
                    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));

                    // Add active class to clicked tab
                    this.classList.add('active');

                    // Hide all tab panes
                    document.querySelectorAll('.tab-pane').forEach(pane => pane.style.display = 'none');

                    // Show the selected tab pane
                    const tabId = this.getAttribute('data-tab');
                    const tabPane = document.getElementById(tabId);
                    if (tabPane) {
                        tabPane.style.display = 'block';
                    }
                });
            });
        }
    }
};

// Make functions available globally
window.initPdfViewer = initPdfViewer;
window.goToPage = goToPage;