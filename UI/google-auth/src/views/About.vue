
<template>
<div>
  <div id="g_id_onload"
         :data-client_id="$store.state.googleConfig.clientId"
>    </div>

</div>

</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
@Component
export default class About extends Vue {
	handleCredentialResponse(response) {
		console.log(response)
	}
}
</script>

<style>
</style>
