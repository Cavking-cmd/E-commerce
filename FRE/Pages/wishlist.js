document.addEventListener('DOMContentLoaded', async () => {
    try {
        const res = await fetch('https://localhost:7180/api/Wishlist/all');
        const result = await res.json();
        const items = result.data || result;
        const container = document.getElementById('wishlist-items');
        if (!items.length) {
            container.innerHTML = '<p>Your wishlist is empty.</p>';
            return;
        }
        container.innerHTML = items.map(item => `
            <div class="bg-white rounded shadow p-4 mb-4 flex justify-between items-center">
                <div>
                    <h3 class="font-bold">${item.productName}</h3>
                    <span class="text-indigo-600 font-semibold">$${item.price}</span>
                </div>
                <button class="bg-red-500 text-white px-3 py-1 rounded" onclick="removeWishlistItem('${item.id}')">Remove</button>
            </div>
        `).join('');
    } catch {
        document.getElementById('wishlist-items').innerHTML = '<p class="text-red-500">Failed to load wishlist.</p>';
    }
});

window.removeWishlistItem = async function(id) {
    try {
        await fetch(`https://localhost:7180/api/Wishlist/${id}`, { method: 'DELETE' });
        location.reload();
    } catch {
        alert('Failed to remove item.');
    }
};