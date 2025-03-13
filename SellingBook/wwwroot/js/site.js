async function addProduct(cartItemObj, onSuccessCallBack, onFailedCallBack) {
    fetch("/Cart/AddCartItem", {
        "method": "Post",
        "headers": {
            "Content-Type": "application/json"
        },
        "body": JSON.stringify(cartItemObj)
    })
        .then(async (response) => {
            await onSuccessCallBack(response);
        })
        .catch(error => {
            onFailedCallBack(error);
        })
}