<template>
    <div class="form-signin">
        <font-awesome-icon :icon="['fas', 'gamepad']" class="logo"/>
        <h1 class="h3 mb-3 fw-normal">Recent Games</h1>
    </div>
    <table class="table table-striped rounded-3 shadow">
        <thead>
        <tr>
            <th scope="col">Date</th>
            <th scope="col">Game type</th>
            <th scope="col">Players</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(game, index) in results" :key="index">
            <td>{{ game.startTime }}</td>
            <td>{{ game.gameType }}</td>
            <td>
                    <span
                        v-for="player in game.userNames" :key="player"
                    >{{ player }},
                    </span>
            </td>
        </tr>
        </tbody>
    </table>
    <div></div>
</template>

<script lang="ts">
import {defineComponent} from "vue";
import * as Res from "../../domain/result/result";

export default defineComponent({
    name: "RecentGames",
    emits: ["fetchUserName"],
    props: ["userName"],
    data: function () {
        return {
            results: new Object() as Res.RecentGames
        };
    },


    methods: {
        getRecentGames: async function () {
            let response = await Res.getRecentGames();
            if (response.status === 200) {
                this.results = response.body.data;
            }
        }
    },
    created: async function () {
        this.getRecentGames();
    }

});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
