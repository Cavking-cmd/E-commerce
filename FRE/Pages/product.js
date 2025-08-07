document.addEventListener('DOMContentLoaded', async () => {
    try {
        const res = await fetch('https://localhost:7180/api/Product');
        const result = await res.json();
        const products = result.data || result;
        const container = document.getElementById('products-list');
        container.innerHTML = products.map(product => `
            <div class="bg-white rounded shadow p-4">
                <h3 class="text-lg font-bold">${product.name}</h3>
                <p>${product.description}</p>
                <span class="text-indigo-600 font-semibold">$${product.price}</span>
                <a href="product-detail.html?id=${product.id}" class="block mt-2 text-blue-500 underline">View Details</a>
            </div>
        `).join('');
    } catch (err) {
        document.getElementById('products-list').innerHTML = '<p class="text-red-500">Failed to load products.</p>';
    }
});