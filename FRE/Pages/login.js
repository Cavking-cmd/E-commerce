document.getElementById('login-form').addEventListener('submit', async function(e) {
    e.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const errorMessage = document.getElementById('error-message');
    errorMessage.textContent = '';

    try {
        const res = await fetch('https://localhost:7180/api/controller/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        const data = await res.json();
        if (res.ok && data.status) {
            localStorage.setItem('userId', data.data.id);
            window.location.href = 'index.html';
        } else {
            errorMessage.textContent = data.message || 'Login failed';
        }
    } catch (err) {
        errorMessage.textContent = 'Network error';
    }
});