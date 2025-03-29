async function addProduct(cartItemObj, onSuccess, onError) {
    try {
        const response = await fetch("/Customer/Product/AddToCart", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(cartItemObj)
        });

        if (!response.ok) {
            onError(response.statusText);
        } else {
            onSuccess(response);
        }
    } catch (error) {
        onError(error);
    }
}