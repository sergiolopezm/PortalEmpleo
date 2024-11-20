// Namespace para funciones globales
const PortalEmpleo = {
    // Configuración de mensajes
    messages: {
        success: {
            icon: 'fas fa-check-circle',
            className: 'alert-success'
        },
        error: {
            icon: 'fas fa-exclamation-circle',
            className: 'alert-danger'
        },
        warning: {
            icon: 'fas fa-exclamation-triangle',
            className: 'alert-warning'
        },
        info: {
            icon: 'fas fa-info-circle',
            className: 'alert-info'
        }
    },

    // Mostrar mensajes tipo toast
    showToast: function (message, type = 'success') {
        const config = this.messages[type];
        const toast = `
            <div class="toast-container position-fixed top-0 end-0 p-3">
                <div class="toast align-items-center border-0 fade show ${config.className}" role="alert">
                    <div class="d-flex">
                        <div class="toast-body">
                            <i class="${config.icon} me-2"></i>${message}
                        </div>
                        <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast"></button>
                    </div>
                </div>
            </div>`;

        document.body.insertAdjacentHTML('beforeend', toast);
        const toastEl = document.querySelector('.toast');
        const bsToast = new bootstrap.Toast(toastEl, { delay: 3000 });
        bsToast.show();

        // Limpiar el DOM después de ocultarse
        toastEl.addEventListener('hidden.bs.toast', () => {
            toastEl.parentElement.remove();
        });
    },

    // Mostrar mensajes en contenedor específico
    showMessage: function (containerId, message, type = 'success') {
        const config = this.messages[type];
        const alert = `
            <div class="alert ${config.className} alert-dismissible fade show" role="alert">
                <i class="${config.icon} me-2"></i>${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>`;

        const container = document.getElementById(containerId);
        if (container) {
            container.innerHTML = alert;
        }
    },

    // Configuración de validación
    validationSettings: {
        errorClass: 'is-invalid',
        successClass: 'is-valid',
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            error.insertAfter(element);
        },
        highlight: function (element) {
            $(element).addClass('is-invalid').removeClass('is-valid');
        },
        unhighlight: function (element) {
            $(element).removeClass('is-invalid').addClass('is-valid');
        },
        messages: {
            required: "Este campo es obligatorio",
            email: "Por favor, ingrese un correo electrónico válido",
            minlength: "Por favor, ingrese al menos {0} caracteres",
            maxlength: "Por favor, no ingrese más de {0} caracteres"
        }
    },

    // Validar formulario
    validateForm: function (formId, options = {}) {
        const form = document.getElementById(formId);
        if (!form) return;

        const settings = { ...this.validationSettings, ...options };

        // Inicializar validación de jQuery
        $(form).validate(settings);

        // Agregar validación HTML5
        form.addEventListener('submit', function (e) {
            if (!this.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            this.classList.add('was-validated');
        });

        return $(form);
    },

    // Manejar solicitudes AJAX
    ajaxRequest: function (url, options = {}) {
        const defaults = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            showLoader: true,
            successMessage: null,
            errorMessage: 'Ha ocurrido un error al procesar la solicitud'
        };

        const settings = { ...defaults, ...options };

        if (settings.showLoader) {
            this.showLoader();
        }

        return fetch(url, settings)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText);
                }
                return response.json();
            })
            .then(data => {
                if (settings.successMessage) {
                    this.showToast(settings.successMessage, 'success');
                }
                return data;
            })
            .catch(error => {
                this.showToast(settings.errorMessage, 'error');
                throw error;
            })
            .finally(() => {
                if (settings.showLoader) {
                    this.hideLoader();
                }
            });
    },

    // Mostrar loader
    showLoader: function () {
        if (!document.getElementById('globalLoader')) {
            const loader = `
                <div id="globalLoader" class="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center bg-white bg-opacity-75" style="z-index: 9999;">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>`;
            document.body.insertAdjacentHTML('beforeend', loader);
        }
    },

    // Ocultar loader
    hideLoader: function () {
        const loader = document.getElementById('globalLoader');
        if (loader) {
            loader.remove();
        }
    },

    // Inicializar todas las funcionalidades
    init: function () {
        // Inicializar tooltips de Bootstrap
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });

        // Manejar mensajes TempData
        const tempDataSuccess = document.querySelector('[data-temp-success]');
        if (tempDataSuccess) {
            this.showToast(tempDataSuccess.dataset.tempSuccess, 'success');
        }

        const tempDataError = document.querySelector('[data-temp-error]');
        if (tempDataError) {
            this.showToast(tempDataError.dataset.tempError, 'error');
        }
    }
};

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function () {
    PortalEmpleo.init();
});