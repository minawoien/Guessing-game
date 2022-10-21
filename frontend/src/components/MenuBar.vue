<template>
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary sticky-top">
        <div class="container-fluid">
            <font-awesome-icon :icon="['fas', 'gamepad']" class="menubarLogo"/>
            <a class="navbar-brand" href="#">Guessing Game</a>
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <router-link to="/" class="nav-link">Game</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/leaderboard" class="nav-link">Leaderboards</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/recentgames" class="nav-link">Recent games</router-link>
                    </li>
                </ul>
                <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a href="#" class="nav-link disabled text-light" v-if="userName">Hello, {{ userName }}</a>
                    </li>
                    <li class="nav-item" v-if="!userName">
                        <router-link to="/login" class="nav-link">
                            <font-awesome-icon :icon="['fas', 'user-check']" class="menubarIcon"/>
                            Login
                        </router-link>
                    </li>
                    <li class="nav-item" v-if="userName">
                        <a href="#" v-on:click="logout" class="nav-link">
                            <font-awesome-icon :icon="['fas', 'user-minus']" class="menubarIcon"/>
                            Logout</a>
                    </li>
                    <li class="nav-item" v-if="!userName">
                        <router-link to="/register" class="nav-link">
                            <font-awesome-icon :icon="['fas', 'user-plus']" class="menubarIcon"/>
                            Register
                        </router-link>
                    </li>
                </ul>
            </div>
    </nav>
</template>

<script lang="ts">
import {defineComponent} from 'vue';
import {logoutUserRequest} from "../domain/auth/logout"


export default defineComponent({
    name: 'MenuBar',
    props: ["userName"],
    emits: ['fetchUserName'],

    methods: {
        logout: async function () {
            let status = await logoutUserRequest();
            if (status === 200) {
                this.$emit('fetchUserName', "");
                await this.$router.push('/');
            }
        }
    },
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>


</style>
