function renderProducts(products) {
    const productList = document.getElementById('productList');
    if (!productList) return;

    // Làm mờ khi render
    productList.classList.remove('fade-in');
    productList.classList.add('fade-out');

    setTimeout(() => {
        if (!products || products.length === 0) {
            productList.innerHTML = '<p class="loading-message">Không tìm thấy sản phẩm nào.</p>';
        } else {
            productList.innerHTML = products.map(product => `
                <div class="col-md-4 mb-4">
                    <div class="card h-100">
                        <img src="${product.imageUrl}" class="card-img-top" alt="${product.productName}" style="height: 200px; object-fit: cover;" />
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title">${product.productName}</h5>
                            <p class="card-text text-muted">Danh mục: ${product.categoryName}</p>
                            <p class="card-text text-danger fw-bold">${product.productPrice.toLocaleString()} VNĐ</p>
                            <a href="/Customer/Product/Display/${product.productId}" class="btn btn-primary mt-auto">Xem chi tiết</a>
                        </div>
                    </div>
                </div>
            `).join('');
        }

        productList.classList.remove('fade-out');
        productList.classList.add('fade-in');
    }, 200);
}

function fetchAndRenderProducts(sort = "") {
    const productList = document.getElementById('productList');
    const resetSortBtn = document.getElementById('resetSortBtn');

    if (!productList) return;

    const keyword = document.body.dataset.keyword || '';
    const categoryId = document.getElementById('categoryFilter')?.value || '';
    const page = 1;
    const pageSize = 9;

    let url = `/api/products/sort?keyword=${encodeURIComponent(keyword)}${categoryId ? `&categoryId=${categoryId}` : ''}&page=${page}&pageSize=${pageSize}`;
    if (sort) {
        url += `&sort=${sort}`;
    }

    fetch(url)
        .then(res => {
            if (!res.ok) throw new Error(`HTTP error! Status: ${res.status}`);
            return res.json();
        })
        .then(data => {
            const products = data.products || [];
            renderProducts(products);
        })
        .catch(err => {
            console.error('Lỗi khi tải sản phẩm:', err);
            productList.innerHTML = '<p class="text-danger">Có lỗi xảy ra khi tải sản phẩm.</p>';
        })
        .finally(() => {
            productList.classList.remove('fade-out');
            productList.classList.add('fade-in');
        });
}

document.addEventListener('DOMContentLoaded', function () {
    const sortFilter = document.getElementById('sortFilter');
    const resetSortBtn = document.getElementById('resetSortBtn');

    if (sortFilter) {
        sortFilter.addEventListener('change', function () {
            const sort = this.value;
            fetchAndRenderProducts(sort);
        });
    }

    if (resetSortBtn) {
        resetSortBtn.addEventListener('click', function () {
            fetchAndRenderProducts(""); // load mặc định
        });
    }
});
