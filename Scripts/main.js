var Sortable = {
    baseUrl: '',
    sortBy: 0,
    searchTerm: '',
    Search() {
        var searchKey = $('#txtSearch').val();
        window.location.href = Sortable.baseUrl + searchKey;
    },
    Sort(sortBy) {
        window.location.href = Sortable.baseUrl + "/?sortBy=" + sortBy;
    }
};

/* 
    The addressAutocomplete takes a container element (div) as a parameter
*/
function addressAutocomplete(containerElement) {
    // create input element
    var inputElement = document.createElement("input");
    inputElement.setAttribute("type", "text");
    inputElement.setAttribute("placeholder", "Enter an address here");
    containerElement.appendChild(inputElement);
}

addressAutocomplete(document.getElementById("autocomplete-container"));

function addressAutocomplete(containerElement) {
  ...

    /* Active request promise reject function. To be able to cancel the promise when a new request comes */
    var currentPromiseReject;

    /* Execute a function when someone writes in the text field: */
    inputElement.addEventListener("input", function (e) {
        var currentValue = this.value;

        // Cancel previous request promise
        if (currentPromiseReject) {
            currentPromiseReject({
                canceled: true
            });
        }

        if (!currentValue) {
            return false;
        }

        /* Create a new promise and send geocoding request */
        var promise = new Promise((resolve, reject) => {
            currentPromiseReject = reject;

            var apiKey = "47f523a46b944b47862e39509a7833a9";
            var url = `https://api.geoapify.com/v1/geocode/autocomplete?text=${encodeURIComponent(currentValue)}&limit=5&apiKey=${apiKey}`;

            fetch(url)
                .then(response => {
                    // check if the call was successful
                    if (response.ok) {
                        response.json().then(data => resolve(data));
                    } else {
                        response.json().then(data => reject(data));
                    }
                });
        });

        promise.then((data) => {
            // we will process data here
        }, (err) => {
            if (!err.canceled) {
                console.log(err);
            }
        });
    });
}

/* 
    The addressAutocomplete takes as parameters:
  - a container element (div)
  - callback to notify about address selection
*/
function addressAutocomplete(containerElement, callback) {
  ...

    inputElement.addEventListener("input", function (e) {
    ...

    /* Create a new promise and send geocoding request */
    var promise = new Promise((resolve, reject) => {...});

promise.then((data) => {
    currentItems = data.features;
      ...
    /* For each item in the results */
    data.features.forEach((feature, index) => {
        /* Create a DIV element for each element: */
        var itemElement = document.createElement("DIV");
        /* Set formatted address as item value */
        itemElement.innerHTML = feature.properties.formatted;

        /* Set the value for the autocomplete text field and notify: */
        itemElement.addEventListener("click", function (e) {
            inputElement.value = currentItems[index].properties.formatted;
            callback(currentItems[index]);
            /* Close the list of autocompleted values: */
            closeDropDownList();
        });

        autocompleteItemsElement.appendChild(itemElement);
    });
    }, (err) => {... });
  });
  ...
}

function addressAutocomplete(containerElement, callback) {
  ...
    /* Execute a function when someone writes in the text field: */
    inputElement.addEventListener("input", function (e) {...});

/* Add support for keyboard navigation */
inputElement.addEventListener("keydown", function (e) {
    var autocompleteItemsElement = containerElement.querySelector(".autocomplete-items");
    if (autocompleteItemsElement) {
        var itemElements = autocompleteItemsElement.getElementsByTagName("div");
        if (e.keyCode == 40) {
            e.preventDefault();
            /*If the arrow DOWN key is pressed, increase the focusedItemIndex variable:*/
            focusedItemIndex = focusedItemIndex !== itemElements.length - 1 ? focusedItemIndex + 1 : 0;
            /*and and make the current item more visible:*/
            setActive(itemElements, focusedItemIndex);
        } else if (e.keyCode == 38) {
            e.preventDefault();

            /*If the arrow UP key is pressed, decrease the focusedItemIndex variable:*/
            focusedItemIndex = focusedItemIndex !== 0 ? focusedItemIndex - 1 : focusedItemIndex = (itemElements.length - 1);
            /*and and make the current item more visible:*/
            setActive(itemElements, focusedItemIndex);
        } else if (e.keyCode == 13) {
            /* If the ENTER key is pressed and value as selected, close the list*/
            e.preventDefault();
            if (focusedItemIndex > -1) {
                closeDropDownList();
            }
        }
    } else {
        if (e.keyCode == 40) {
            /* Open dropdown list again */
            var event = document.createEvent('Event');
            event.initEvent('input', true, true);
            inputElement.dispatchEvent(event);
        }
    }
});

function setActive(items, index) {
    if (!items || !items.length) return false;

    for (var i = 0; i < items.length; i++) {
        items[i].classList.remove("autocomplete-active");
    }

    /* Add class "autocomplete-active" to the active element*/
    items[index].classList.add("autocomplete-active");

    // Change input value and notify
    inputElement.value = currentItems[index].properties.formatted;
    callback(currentItems[index]);
}

function closeDropDownList() {
    ...
    focusedItemIndex = -1;
}
}

function addressAutocomplete(containerElement, callback) {
  ...

    /* Close the autocomplete dropdown when the document is clicked. 
        Skip, when a user clicks on the input field */
    document.addEventListener("click", function (e) {
        if (e.target !== inputElement) {
            closeDropDownList();
        } else if (!containerElement.querySelector(".autocomplete-items")) {
            // open dropdown list again
            var event = document.createEvent('Event');
            event.initEvent('input', true, true);
            inputElement.dispatchEvent(event);
        }
    });
}
