async function addProduct(cartItemObj, onSuccessCallBack, onFailedCallBack) {
    try {
        const response = await fetch("/Customer/Cart/AddCartItem", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(cartItemObj)
        });

        // Optional: basic status check
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        await onSuccessCallBack(response);
    } catch (error) {
        onFailedCallBack(error);
    }
}
