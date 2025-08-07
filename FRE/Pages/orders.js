document.addEventListener('DOMContentLoaded', async () => {
    try {
        const res = await fetch('https://localhost:7180/api/Order/all');
        const result = await res.json();
        const orders = result.data || result;
        const container = document.getElementById('orders-list');
        if (!orders.length) {
            container.innerHTML = '<p>You have no orders.</p>';
            return;
        }
        container.innerHTML = orders.map(order => `
            <div class="bg-white rounded shadow p-4 mb-4">
                <h3 class="font-bold">Order #${order.id}</h3>
                <p>Date: ${order.date}</p>
                <p>Status: ${order.status}</p>
                <span class="text-indigo-600 font-semibold">Total: $${order.total}</span>
            </div>
        `).join('');
    } catch {
        document.getElementById('orders-list').innerHTML = '<p class="text-red-500">Failed to load orders.</p>';
    }
});