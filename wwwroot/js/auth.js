/* ==========================================================================
   AssetHub - Authentication Interactive Controller
   ========================================================================== */

document.addEventListener('DOMContentLoaded', function () {
  // 1. Password Visibility Toggle
  const toggleBtn = document.getElementById('togglePassword');
  const passwordInput = document.getElementById('passwordInput');
  const toggleIcon = document.getElementById('togglePasswordIcon');

  if (toggleBtn && passwordInput && toggleIcon) {
    toggleBtn.addEventListener('click', function () {
      const isPassword = passwordInput.getAttribute('type') === 'password';
      passwordInput.setAttribute('type', isPassword ? 'text' : 'password');
      
      if (isPassword) {
        toggleIcon.classList.remove('bi-eye');
        toggleIcon.classList.add('bi-eye-slash');
        toggleBtn.setAttribute('aria-label', 'Hide password');
      } else {
        toggleIcon.classList.remove('bi-eye-slash');
        toggleIcon.classList.add('bi-eye');
        toggleBtn.setAttribute('aria-label', 'Show password');
      }
      passwordInput.focus();
    });
  }

  // 2. Demo Credentials Quick Fill
  const fillAdminBtn = document.getElementById('fillAdminDemo');
  const fillManagerBtn = document.getElementById('fillManagerDemo');
  const emailInput = document.getElementById('emailInput');

  function populateDemo(email, pwd) {
    if (emailInput) {
      emailInput.value = email;
      emailInput.dispatchEvent(new Event('input', { bubbles: true }));
    }
    if (passwordInput) {
      passwordInput.value = pwd;
      passwordInput.dispatchEvent(new Event('input', { bubbles: true }));
    }
  }

  if (fillAdminBtn) {
    fillAdminBtn.addEventListener('click', function () {
      populateDemo('admin@assethub.com', 'Admin@1234');
      highlightFieldFeedback();
    });
  }

  if (fillManagerBtn) {
    fillManagerBtn.addEventListener('click', function () {
      populateDemo('manager@assethub.com', 'Manager@1234');
      highlightFieldFeedback();
    });
  }

  function highlightFieldFeedback() {
    [emailInput, passwordInput].forEach(function(el) {
      if (!el) return;
      el.style.borderColor = '#38bdf8';
      setTimeout(() => {
        el.style.borderColor = '';
      }, 600);
    });
  }

  // 3. Button Loading Animation on Submit
  const loginForm = document.getElementById('loginForm');
  const submitBtn = document.getElementById('submitBtn');

  if (loginForm && submitBtn) {
    loginForm.addEventListener('submit', function () {
      // Check if form is valid (if HTML5 validation passes)
      if (loginForm.checkValidity()) {
        submitBtn.disabled = true;
        const spinnerHtml = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Authenticating...`;
        submitBtn.innerHTML = spinnerHtml;
      }
    });
  }
});
