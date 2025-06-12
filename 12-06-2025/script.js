const users = [{
    username: "user1",
    email: "user1@example.com"
}, 
{
    username: "user2",
    email: "user2@example.com"
}];

const awaitContainer = document.getElementById("await-container");
const promiseContainer = document.getElementById("promise-container");
const callbackContainer = document.getElementById("callback-container");

async function fetchDataUsingAsyncAwait() {
    try {
        const result = await new Promise((resolve, reject) => {
            setTimeout(() => {
                const success = users.length > 0; 
                if (success) {
                    resolve("Data fetched successfully using Async/Await");
                } else {
                    reject("Failed to fetch data");
                }
            }, 2000);
        });
        return result;
    } catch (error) {
        console.error(error);
    }
}

function fetchDataUsingCallback(callback) {
  setTimeout(() => {
    const success = users.length > 0; 
    if (success) {
      callback(null, "Data fetched successfully using Callback");
    } else {
      callback("Failed to fetch data", null);
    }
  }, 1000);
}

function fetchDataUsingPromise() {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const success = users.length > 0; 
      if (success) {
        resolve("Data fetched successfully using Promise");
      } else {
        reject("Failed to fetch data");
      }
    }, 3000);
  });
}

function renderHTML(users, container){
    console.log("Rendering HTML");
    console.log(users);
    users.forEach(user => {
        const unorderedList = document.createElement("ul");
        const name = document.createElement("li");
        const email = document.createElement("li");
        name.textContent = `Username: ${user.username}`;
        email.textContent = `Email: ${user.email}`;
        unorderedList.appendChild(name);
        unorderedList.appendChild(email);
        container.appendChild(unorderedList);
    });
}

function fetchAndRenderByAsyncAwait() {
    fetchDataUsingAsyncAwait()
        .then(result => {
            console.log(result);
            renderHTML(users, awaitContainer);
        })
        .catch(error => {
            console.error(error);
        });
}

function fetchAndRenderByCallback(){
     fetchDataUsingCallback((error, result) => {
            if (error) {
                console.error(error);
            } else {
                console.log(result);
                renderHTML(users, callbackContainer);
            }
        }
    );
}

function fetchAndRenderByPromise() {
    fetchDataUsingPromise()
        .then(result => {
            console.log(result);
            renderHTML(users, promiseContainer);
        })
        .catch(error => {
            console.error(error);
        });
}

function fetchAndRenderAll(){
    fetchAndRenderByAsyncAwait();
    fetchAndRenderByCallback();
    fetchAndRenderByPromise();
    fetchAndRenderByAsyncAwait();
    fetchAndRenderByCallback();
    fetchAndRenderByPromise();
}