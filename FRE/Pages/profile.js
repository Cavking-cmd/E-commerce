document.addEventListener('DOMContentLoaded', async () => {
    try {
        // Replace with actual user ID logic
        const userId = localStorage.getItem('userId');
        if (!userId) {
            document.getElementById('profile-info').innerHTML = '<p>Please log in.</p>';
            return;
        }
        const res = await fetch(`https://localhost:7180/api/UserProfile/${userId}`);
        const result = await res.json();
        const profile = result.data || result;
        document.getElementById('profile-info').innerHTML = `
            <div class="bg-white rounded shadow p-6">
                <h2 class="text-xl font-bold mb-2">${profile.fullName}</h2>
                <p>Email: ${profile.email}</p>
                <p>Phone: ${profile.phoneNumber}</p>
            </div>
        `;
    } catch {
        document.getElementById('profile-info').innerHTML = '<p class="text-red-500">Failed to load profile.</p>';
    }
});