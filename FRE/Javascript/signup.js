// API endpoint for user registration
const API_BASE_URL = 'https://localhost:7180';

// Form elements
const signupForm = document.getElementById('signupForm');
const errorMessage = document.getElementById('errorMessage');

// Handle form submission
signupForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    
    // Clear previous error messages
    errorMessage.textContent = '';
    errorMessage.classList.add('hidden');
    
    // Get form data
    const formData = new FormData(signupForm);
    const userData = {
        userName: formData.get('userName'),
        email: formData.get('email'),
        password: formData.get('password'),
        confirmPassword: formData.get('confirmPassword'),
        firstName: formData.get('firstName'),
        lastName: formData.get('lastName'),
        phoneNumber: formData.get('phoneNumber'),
        addressLine: formData.get('addressLine'),
        city: formData.get('city'),
        state: formData.get('state'),
        postalCode: formData.get('postalCode'),
        country: formData.get('country')
    };
    
    // Validate form data
    if (!validateForm(userData)) {
        return;
    }
    
    try {
        // Show loading state
        const submitButton = signupForm.querySelector('button[type="submit"]');
        submitButton.textContent = 'Creating Account...';
        submitButton.disabled = true;
        
        // Send registration request
        const response = await apiFetch('/api/auth/register', {
            method: 'POST',
            body: JSON.stringify(userData)
        });
        
        // Handle successful registration
        alert('Account created successfully! Please log in.');
        window.location.href = 'login.html';
        
    } catch (error) {
        // Handle registration errors
        errorMessage.textContent = error.message || 'Registration failed. Please try again.';
        errorMessage.classList.remove('hidden');
        
        // Reset button state
        const submitButton = signupForm.querySelector('button[type="submit"]');
        submitButton.textContent = 'Create Account';
        submitButton.disabled = false;
    }
});

// Form validation function
function validateForm(data) {
    // Check required fields
    if (!data.userName || !data.email || !data.password || !data.confirmPassword || 
        !data.firstName || !data.lastName || !data.phoneNumber || !data.addressLine || 
        !data.city || !data.state || !data.postalCode || !data.country) {
        showError('All fields are required');
        return false;
    }
    
    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(data.email)) {
        showError('Please enter a valid email address');
        return false;
    }
    
    // Validate password length
    if (data.password.length < 6) {
        showError('Password must be at least 6 characters long');
        return false;
    }
    
    // Validate password match
    if (data.password !== data.confirmPassword) {
        showError('Passwords do not match');
        return false;
    }
    
    // Validate phone number (basic format)
    const phoneRegex = /^[0-9+\-\s()]+$/;
    if (!phoneRegex.test(data.phoneNumber)) {
        showError('Please enter a valid phone number');
        return false;
    }
    
    return true;
}

// Show error message
function showError(message) {
    errorMessage.textContent = message;
    errorMessage.classList.remove('hidden');
}

// API fetch function
async function apiFetch(options = {}) {
    const res = await fetch(`https://localhost:7180/api/customer`, {
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', ...options.headers },
        ...options
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}
