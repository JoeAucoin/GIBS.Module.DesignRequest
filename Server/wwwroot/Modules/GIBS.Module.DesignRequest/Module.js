/* Module Script */
var GIBS = GIBS || {};

GIBS.DesignRequest = {


    _isLoading: false,
    _isRendering: false,

    downloadFile: function (filename, base64Content, contentType) {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:" + contentType + ";base64," + base64Content;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    },

    loadPayPalSdk: function (url, dotNetRef) {
        // 1. If PayPal is already loaded, just render
        if (window.paypal) {
            GIBS.DesignRequest.renderPayPalButtons(dotNetRef);
            return;
        }

        // 2. Prevent multiple listeners/intervals if already loading
        if (GIBS.DesignRequest._isLoading) {
            return;
        }
        GIBS.DesignRequest._isLoading = true;

        var scriptId = "paypal-sdk-script";
        var script = document.getElementById(scriptId);

        if (!script) {
            // 3. Create and append the script tag
            script = document.createElement("script");
            script.id = scriptId;
            script.src = url;
            script.async = true;

            script.onload = function () {
                GIBS.DesignRequest._isLoading = false;
                GIBS.DesignRequest.renderPayPalButtons(dotNetRef);
            };

            script.onerror = function () {
                GIBS.DesignRequest._isLoading = false;
                console.error("Failed to load PayPal SDK from: " + url);
            };

            document.body.appendChild(script);
        } else {
            // 4. Script exists but window.paypal isn't ready yet. Poll for it.
            var checkInterval = setInterval(function () {
                if (window.paypal) {
                    clearInterval(checkInterval);
                    GIBS.DesignRequest._isLoading = false;
                    GIBS.DesignRequest.renderPayPalButtons(dotNetRef);
                }
            }, 100);

            // Stop polling after 10 seconds
            setTimeout(function () {
                if (GIBS.DesignRequest._isLoading) {
                    clearInterval(checkInterval);
                    GIBS.DesignRequest._isLoading = false;
                }
            }, 10000);
        }
    },

    renderPayPalButtons: function (dotNetRef) {
        // 5. Prevent concurrent rendering which causes "container element removed" errors
        if (GIBS.DesignRequest._isRendering) {
            return;
        }

        const container = document.getElementById('paypal-button-container');
        if (!container) {
            // Container might be gone if user navigated away
            return;
        }

        GIBS.DesignRequest._isRendering = true;
        container.innerHTML = '';

        if (!window.paypal) {
            console.error('PayPal SDK is not loaded.');
            GIBS.DesignRequest._isRendering = false;
            return;
        }

        const buttons = paypal.Buttons({
            async createOrder() {
                try {
                    const orderId = await dotNetRef.invokeMethodAsync('CreateOrderOnServer');
                    if (orderId) {
                        return orderId;
                    } else {
                        throw new Error('Server-side order creation failed.');
                    }
                } catch (error) {
                    console.error('Error creating PayPal order:', error);
                    alert(`Could not initiate PayPal Checkout. Please try again.\n\nError: ${error.message}`);
                    throw error;
                }
            },

            async onApprove(data) {
                try {
                    const captureResult = await dotNetRef.invokeMethodAsync('CaptureOrderOnServer', data.orderID);

                    if (captureResult && captureResult.error === 'INSTRUMENT_DECLINED') {
                        alert('Your payment was declined. Please try another payment method.');
                    }

                } catch (error) {
                    console.error('Error capturing order:', error);
                    alert('An error occurred while processing your payment.');
                }
            },

            onError: function (err) {
                console.error('PayPal Button Error:', err);
            }
        });

        // 6. Render returns a promise; handle it to release the lock
        buttons.render('#paypal-button-container')
            .then(() => {
                GIBS.DesignRequest._isRendering = false;
            })
            .catch((err) => {
                console.error('Failed to render PayPal buttons:', err);
                GIBS.DesignRequest._isRendering = false;
            });
    }

};