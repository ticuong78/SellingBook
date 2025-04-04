<script>
    document.getElementById('searchBox').addEventListener('input', function () {
    const keyword = this.value.trim();

    if (keyword.length === 0) {
        document.getElementById('searchResults').innerHTML = '';
    return;
    }

    fetch(`/api/products/search?keyword=${encodeURIComponent(keyword)}`)
        .then(res => res.json())
        .then(data => {
        let html = '';
            data.forEach(p => {
        html += `
                    <div class="col-md-4 mb-3">
                        <div class="card">
                            <img src="/images/products/${p.image}" class="card-img-top" alt="${p.productName}">
                            <div class="card-body">
                                <h5 class="card-title">${p.productName}</h5>
                                <p class="card-text">${p.price.toLocaleString()} VND</p>
                            </div>
                        </div>
                    </div>
                `;
            });
    document.getElementById('searchResults').innerHTML = html;
        });
});
</script>
