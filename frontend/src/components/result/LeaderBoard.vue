<template>
    <div class="leaderboard-form">
        <font-awesome-icon :icon="['fas', 'gamepad']" class="logo"/>
        <h1 class="h3 mb-3 fw-normal">
            Pick which leaderboard you want to see
        </h1>
        <div>
            <button
                type="button"
                v-on:click="getResult"
                class="btn btn-outline-primary m-3"
            >
                Leaderboard
            </button>
            <button
                type="button"
                v-on:click="getTeamResult"
                class="btn btn-outline-danger m-3"
            >
                Team Leaderboard
            </button>
        </div>
    </div>
    <div v-if="showLeaderboard">
        <h1 class="h3 mb-3 fw-normal">Leaderboards</h1>
        <table class="table table-striped rounded-3 shadow">
            <thead>
            <tr>
                <th scope="col">Rank</th>
                <th scope="col">Player</th>
                <th scope="col" class="text-end">Score</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(game, index) in results" :key="index">
                <th scope="row">{{ index + 1 }}</th>
                <td>{{ game.userName }}</td>
                <td class="text-end">{{ game.score }}</td>
            </tr>
            </tbody>
        </table>
    </div>
    <div v-if="showTeamLeaderboard">
        <h1 class="h3 mb-3 fw-normal">Team Leaderboards</h1>
        <table class="table table-striped rounded-3 shadow">
            <thead>
            <tr>
                <th scope="col">Rank</th>
                <th scope="col">Players</th>
                <th scope="col" class="text-end">Score</th>
            </tr>
            </thead>
            <tbody>
            <tr
                v-for="(game, index) in teamResults"
                :key="index"
            >
                <th scope="row">{{ index + 1 }}</th>
                <td>
                        <span
                            v-for="player in game.userNames" :key="player"
                        >{{ player }},
                        </span>
                </td>
                <td class="text-end">{{ game.score }}</td>

            </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
import {defineComponent} from "vue";
import * as Res from "../../domain/result/result";

export default defineComponent({
    emits: ["fetchUserName"],
    name: "LeaderBoard",
    props: ["userName"],
    data: function () {
        return {
            results: new Object() as Res.Leaderboards,
            teamResults: new Object() as Res.TeamLeaderboards,
            showLeaderboard: false,
            showTeamLeaderboard: false
        };
    },
    methods: {
        getResult: async function () {
            this.showLeaderboard = true;
            this.showTeamLeaderboard = false;
            let response = await Res.getLeaderboards();
            if (response.status === 200) {
                this.results = response.body.data;
            }
        },
        getTeamResult: async function () {
            this.showLeaderboard = false;
            this.showTeamLeaderboard = true;
            let response = await Res.getTeamLeaderboards();
            if (response.status === 200) {
                this.teamResults = response.body.data;
            }
        }
    }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
