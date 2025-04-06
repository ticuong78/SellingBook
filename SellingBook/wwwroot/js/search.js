const searchBox = document.getElementById('searchBox');
const searchButton = document.getElementById('searchButton');
const resultsDiv = document.getElementById('searchResults');

// Hàm xử lý tìm kiếm và chuyển hướng
function performSearch() {
    const keyword = searchBox.value.trim();
    if (keyword.length > 0) {
        // Chuyển hướng đến trang kết quả tìm kiếm với từ khóa
        window.location.href = `/Customer/Product/Search?keyword=${encodeURIComponent(keyword)}`;
    }
}

// Xử lý khi nhấn nút "Tìm"
searchButton.addEventListener('click', performSearch);

// Xử lý khi nhấn Enter trong thanh tìm kiếm
searchBox.addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault(); // Ngăn form submit mặc định (nếu có)
        performSearch();
    }
});

// Xử lý gợi ý tìm kiếm khi nhập
searchBox.addEventListener('input', function () {
    const keyword = this.value.trim();

    // Ẩn kết quả nếu không có từ khóa
    if (keyword.length === 0) {
        resultsDiv.innerHTML = '';
        resultsDiv.style.display = 'none'; // Ẩn kết quả tìm kiếm khi không có từ khóa
        return;
    }

    // Hiển thị kết quả khi có từ khóa
    resultsDiv.style.display = 'block';

    fetch(`/api/products/search?keyword=${encodeURIComponent(keyword)}`)
        .then(res => res.json())
        .then(data => {
            console.log('Dữ liệu từ API:', data); // Kiểm tra dữ liệu trả về từ API
            if (data.length === 0) {
                resultsDiv.innerHTML = '<p class="text-muted">Không tìm thấy sản phẩm nào.</p>';
                return;
            }

            // Định dạng container kết quả để hiển thị ngay bên dưới thanh tìm kiếm
            resultsDiv.style.position = 'absolute'; // Sử dụng định vị tuyệt đối để căn chỉnh với thanh tìm kiếm
            resultsDiv.style.top = '100%'; // Đặt ngay bên dưới thanh tìm kiếm
            resultsDiv.style.left = '0'; // Căn chỉnh với cạnh trái của thanh tìm kiếm
            resultsDiv.style.width = '100%'; // Khớp với chiều rộng của thanh tìm kiếm
            resultsDiv.style.backgroundColor = '#fff'; // Nền trắng để khớp với kiểu dropdown
            resultsDiv.style.border = '1px solid #ddd'; // Thêm viền để khớp với kiểu dropdown
            resultsDiv.style.borderRadius = '4px'; // Bo góc nhẹ cho container
            resultsDiv.style.boxShadow = '0 2px 4px rgba(0,0,0,0.1)'; // Thêm bóng nhẹ
            resultsDiv.style.zIndex = '1000'; // Đảm bảo hiển thị phía trên các phần tử khác
            resultsDiv.style.padding = '5px 0'; // Giảm padding để khớp với kiểu gọn trong hình
            resultsDiv.style.marginTop = '0'; // Đảm bảo không có khoảng cách thừa đẩy kết quả xuống

            // Hiển thị các sản phẩm dưới dạng danh sách dọc
            resultsDiv.innerHTML = data.map(product => `
                        <a href="/Customer/Product/Display/${product.productId}" class="search-result-item">
                            <div class="card" style="display: flex; flex-direction: row; align-items: center; padding: 8px 10px; border-bottom: 1px solid #eee;">
                                <img src="${product.imageUrl}" class="card-img-top" style="width: 40px; height: 60px; object-fit: cover; margin-right: 10px;" alt="${product.productName}" />
                                <div class="card-body" style="flex: 1; padding: 0;">
                                    <h5 class="card-title" style="font-size: 14px; margin: 0; line-height: 1.2; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${product.productName}</h5>
                                    <p class="card-text text-danger fw-bold" style="font-size: 13px; margin: 3px 0 0 0;">${product.productPrice.toLocaleString()} <u>đ</u></p>
                                </div>
                            </div>
                        </a>
                    `).join('');

            // Xóa viền của sản phẩm cuối cùng để tránh đường viền thừa ở dưới cùng
            const cards = resultsDiv.querySelectorAll('.card');
            if (cards.length > 0) {
                cards[cards.length - 1].style.borderBottom = 'none';
            }
        })
        .catch(err => {
            console.error(err);
            resultsDiv.innerHTML = '<p class="text-danger">Có lỗi xảy ra khi tìm kiếm.</p>';
        });
});

// Ẩn kết quả khi nhấn ra ngoài thanh tìm kiếm hoặc kết quả
document.addEventListener('click', function (event) {
    const searchBox = document.getElementById('searchBox');
    const resultsDiv = document.getElementById('searchResults');
    if (!searchBox.contains(event.target) && !resultsDiv.contains(event.target)) {
        resultsDiv.style.display = 'none';
    }
});