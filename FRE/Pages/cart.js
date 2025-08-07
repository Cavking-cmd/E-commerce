document.addEventListener('DOMContentLoaded', async () => {
    try {
        const res = await fetch('https://localhost:7180/api/CartItem');
        const result = await res.json();
        const items = result.data || result;
        const container = document.getElementById('cart-items');
        if (!items.length) {
            container.innerHTML = '<p>Your cart is empty.</p>';
            return;
        }
        container.innerHTML = items.map(item => `
            <div class="bg-white rounded shadow p-4 mb-4 flex justify-between items-center">
                <div>
                    <h3 class="font-bold">${item.productName}</h3>
                    <p>Quantity: ${item.quantity}</p>
                    <span class="text-indigo-600 font-semibold">$${item.price}</span>
                </div>
                <button class="bg-red-500 text-white px-3 py-1 rounded" onclick="removeCartItem('${item.id}')">Remove</button>
            </div>
        `).join('');
    } catch {
        document.getElementById('cart-items').innerHTML = '<p class="text-red-500">Failed to load cart.</p>';
    }
});

window.removeCartItem = async function(id) {
    try {
        await fetch(`https://localhost:7180/api/CartItem/Id?id=${id}`, { method: 'DELETE' });
        location.reload();
    } catch {
        alert('Failed to remove item.');
    }
};