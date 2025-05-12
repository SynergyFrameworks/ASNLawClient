// site.js - JavaScript interop functions for ASN Law Reference

// Document View Toggle (Grid/List)
window.documentViewToggle = {
    initialize: function () {
        const gridViewBtn = document.getElementById('gridViewBtn');
        const listViewBtn = document.getElementById('listViewBtn');
        const gridView = document.getElementById('gridView');
        const listView = document.getElementById('listView');

        if (gridViewBtn && listViewBtn && gridView && listView) {
            gridViewBtn.addEventListener('click', function () {
                gridViewBtn.classList.add('active');
                listViewBtn.classList.remove('active');
                gridView.style.display = 'grid';
                listView.style.display = 'none';
            });

            listViewBtn.addEventListener('click', function () {
                listViewBtn.classList.add('active');
                gridViewBtn.classList.remove('active');
                listView.style.display = 'flex';
                gridView.style.display = 'none';
            });
        }
    }
};

// Settings success message
window.settingsMessages = {
    showSuccess: function () {
        const successMessage = document.getElementById('successMessage');
        if (successMessage) {
            successMessage.style.display = 'block';

            // Scroll to top to show message
            window.scrollTo({ top: 0, behavior: 'smooth' });

            // Hide message after 3 seconds
            setTimeout(() => {
                successMessage.style.display = 'none';
            }, 3000);
        }
    }
};

// Initialize all components when document is ready
document.addEventListener('DOMContentLoaded', function () {
    if (window.documentViewToggle) window.documentViewToggle.initialize();
    if (window.annotationSidebar) window.annotationSidebar.initialize();
    if (window.adminTabs) window.adminTabs.initialize();
});