<template>
    <MenuBar v-bind:userName="userName" @fetchUserName="setUserName"/>
    <div class="m-3 mx-auto component">
        <router-view v-bind:userName="userName" @fetchUserName="setUserName"/>
    </div>

</template>
<script lang="ts">
import {defineComponent} from 'vue'
import MenuBar from '@/components/MenuBar.vue'
import LeaderBoard from '@/components/result/LeaderBoard.vue'
import {getUsername} from "./domain/auth/userName";
import Login from '@/components/auth/Login.vue'
import Register from '@/components/auth/Register.vue'
import RecentGames from '@/components/result/RecentGames.vue'

import {FontAwesomeIcon, FontAwesomeLayers, FontAwesomeLayersText} from '@fortawesome/vue-fontawesome'


export default defineComponent({
    components: {
        MenuBar,
        LeaderBoard,
        Login,
        Register,
        FontAwesomeIcon,
        FontAwesomeLayers,
        FontAwesomeLayersText,
        RecentGames,
    },
    emits: ['fetchUserName'],
    data: function () {
        return {
            userName: "" as string,
        };
    },
    methods: {
        setUserName: function (value: string): void {
            this.userName = value;
        }
    },
    created: async function () {
        this.setUserName(await getUsername());
    }
})
</script>


<style>
@import url(https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css);
@import url('./css/custom.css');

html,
body {
    height: 100%;
}

body {

    align-items: center;
    background-color: #f5f5f5;


}

.component {
    position: relative;
    width: 60%;
    margin: 100px;

}
</style>
