<template>
  <div>
    <div
      id="g_id_onload"
      data-client_id="617655006081-m1s4h8m8ksk2qqvfckhd9dfnud601i98.apps.googleusercontent.com"
      data-callback="handleCredentialResponse"
    ></div>
    <div class="g_id_signin" data-type="standard"></div>
  </div>
</template>

<script lang="ts">
export default {
  name: "App",
  data() {
    return {
      isInit: false,
      isSignIn: false,
    };
  },

  methods: {
    handleCredentialResponse(response) {
      // decodeJwtResponse() is a custom function defined by you
      // to decode the credential response.

      const responsePayload = this.parseJwt(response.credential);
      console.log(responsePayload);
      console.log("ID: " + responsePayload.sub);
      console.log("Full Name: " + responsePayload.name);
      console.log("Given Name: " + responsePayload.given_name);
      console.log("Family Name: " + responsePayload.family_name);
      console.log("Image URL: " + responsePayload.picture);
      console.log("Email: " + responsePayload.email);
    },
    parseJwt(token) {
      var base64Url = token.split(".")[1];
      var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      var jsonPayload = decodeURIComponent(
        window
          .atob(base64)
          .split("")
          .map(function (c) {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join("")
      );

      return JSON.parse(jsonPayload);
    },
  },
  mounted() {
    //let recaptchaScript = document.createElement("script");

    const log = import("./google");
    // console.log(log);
    // recaptchaScript.setAttribute(
    //   "src",
    //   log
    //   //https://accounts.google.com/gsi/client
    // );
    document.head.appendChild(log);
  },
};
</script>
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
