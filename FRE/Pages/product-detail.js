document.addEventListener('DOMContentLoaded', async () => {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    if (!id) return;
    try {
        const res = await fetch(`https://localhost:7180/api/Product/${id}`);
        const result = await res.json();
        const product = result.data || result;
        document.getElementById('product-detail').innerHTML = `
            <div class="bg-white rounded shadow p-6">
                <h2 class="text-2xl font-bold mb-2">${product.name}</h2>
                <p class="mb-4">${product.description}</p>
                <span class="text-indigo-600 font-semibold text-xl">$${product.price}</span>
                <button id="add-to-cart" class="mt-4 bg-indigo-600 text-white px-4 py-2 rounded">Add to Cart</button>
            </div>
        `;
        document.getElementById('add-to-cart').onclick = async () => {
            try {
                await fetch('https://localhost:7180/api/CartItem', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ productId: product.id, quantity: 1 })
                });
                alert('Added to cart!');
            } catch {
                alert('Failed to add to cart.');
            }
        };
    } catch {
        document.getElementById('product-detail').innerHTML = '<p class="text-red-500">Failed to load product.</p>';
    }
});