export async function apiFetch(endpoint, options = {}) {
    const res = await fetch(`https://localhost:7180${endpoint}`, {
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', ...options.headers },
        ...options
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}