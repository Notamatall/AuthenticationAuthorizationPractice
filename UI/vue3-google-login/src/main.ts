import { createApp } from "vue";
import App from "./App.vue";

import plugin from "./plugin";

const app = createApp(App);

app.use(plugin, {
	clientId: "617655006081-m1s4h8m8ksk2qqvfckhd9dfnud601i98.apps.googleusercontent.com",
});

app.mount("#app");
