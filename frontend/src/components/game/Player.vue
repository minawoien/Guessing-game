<template>
    <div
        class="player-window bg-light shadow-lg pt-5 pb-3 px-5 container rounded-3"
    >
        <div class="image" ref="image" @click="imageClick">
            <img :src="firstFragment.fileName"
                 v-bind:class="{ imgopacity: !firstFragment.unlocked}"
                 id="topimg"/>
            <img
                class="sub"
                v-for="img in restFragments"
                :src="img.fileName"
                v-bind:class="{ imgopacity: !img.unlocked}"
                :key="img"
            />
        </div>
        <!-- remeber to add: v-if="game.role == 0" -->
        <div v-if="game.role == 0 && game.status < 3" class="row py-3 player bg-primary rounded-3 shadow-lg">
            <div class="col text-white">
                <h4>What do you see?</h4>
                <form method="POST" v-on:submit.prevent="postGuess">
                    <input type="text" class="form-control" v-model="guess"/>
                    <button
                        class="my-2 w-100 btn btn-light shadow-lg text-primary"
                        type="submit"
                    >
                        Guess
                    </button>
                </form>
                <button
                    v-if="game.type < 2"
                    class="btn w-100 btn-danger shadow-lg mb-2"
                    @click="unlockFragment"
                >
                    Show more
                </button>
                <button v-on:click="quitGame"
                        class="btn w-100 btn-danger shadow-lg mb-1"
                >Quit Game
                </button>
            </div>
            <div class="col text-white">
                <div>
                    <p v-if="game.status == 2">It's time to make a guess!</p>
                    <p v-if="game.status == 1">Waiting for a new imagefragment.</p>
                </div>
                <h4>Guesses:</h4>
                <span class="my-1" v-for="guess in game.guesses" :key="guess">{{ guess }}, </span>
            </div>
        </div>
        <!--  v-if="game.role == 1" -->
        <div
            v-if="game.role == 1 && game.status < 3"
            class="row py-3 player bg-primary rounded-3 shadow-lg"
        >
            <div class="col text-white">
                <h4>Guesses made:</h4>
                <span class="my-1" v-for="guess in game.guesses" :key="guess">{{ guess }}, </span>
            </div>
            <div class="col text-white">
                <h4>Label: {{ game.imageLabel }}</h4>
                <div v-if="game.status == 1">
                    <p>Click on a fragment</p>
                </div>
                <div v-if="game.status == 2">
                    <p>Waiting on response from guesser</p>
                </div>
                <button v-on:click="quitGame"
                        class="btn w-100 btn-danger shadow-lg mb-1"
                >Quit Game
                </button>
            </div>
        </div>
        <div
            v-if="game.status > 2"
            class="row py-3 player bg-primary rounded-3 shadow-lg"
        >
            <div class="col text-white">
                <h4>Game is finished!</h4>
                <h6> Correct guess: {{ game.imageLabel }}</h6>

            </div>
            <div class="col text-white">
                <h4>Winner is: {{ game.winner }}</h4>
                <button v-on:click="quitGame"
                        class="btn w-100 btn-danger shadow-lg mb-1"
                >Quit Game
                </button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import {defineComponent} from "vue";
import * as Game from "@/domain/game/Game";
import {Fragment} from "@/domain/game/Game";
import * as Pixel from "@/domain/game/clickedPixel";

export default defineComponent({
    name: "Proposer",
    data: function () {
        return {
            game: new Object() as Game.Game,
            image: new Object() as Fragment[],
            guess: "" as string,
            polling: 0
        };
    },

    computed: {
        firstFragment(): Fragment {
            if (this.image.length > 0) {
                return this.image[0];
            }
            return new Object() as Fragment;
        },
        restFragments(): Fragment[] {
            if (this.image.length > 1) {
                return this.image.slice(1, this.image.length);
            }
            return new Array<Fragment>();
        }
    },
    methods: {
        quitGame: async function () {
            let response = await fetch("/Game/Quit");
            if (response.status == 200) {
                clearInterval(this.polling);
                this.$emit("quitGame");


            }
        },
        getGame: async function () {
            let response = await Game.getGame();
            if (response.status == 200) {
                this.game = response.body.data;
            }
        },
        getImage: async function () {
            let response = await Game.getImage(this.game.id);
            if (response.status == 200) {
                this.image = response.body.data;
            }
        },
        poll: async function () {
            this.polling = setInterval(async () => {
                let unlocked = this.game.unlockedFragments
                await this.getGame();
                if (this.game.unlockedFragments != unlocked) {
                    await this.getImage();
                }
            }, 1000);
        },
        imageClick(event: MouseEvent) {
            let coordinates = {
                x: event.offsetX,
                y: event.offsetY
            } as Pixel.dimension;
            //could not get ref to work.
            let imgElement = document.getElementById(
                "topimg"
            ) as HTMLImageElement;
            let ratio = Pixel.calcRatio(
                imgElement.width,
                imgElement.height,
                imgElement.naturalWidth,
                imgElement.naturalHeight
            );
            let pixel = Pixel.clickedPixel(coordinates.x, coordinates.y, ratio);
            this.proposeFragment(pixel);
        },

        postGuess: async function () {
            await Game.postGuess({Value: this.guess});

            this.guess = "";
        },
        unlockFragment: async function () {
            let response = await Game.unlocktFragment();
            if (response == 200) {
                await this.getImage();
            }
        },
        proposeFragment: async function (pixel: Pixel.dimension) {
            let response = await Game.proposeFragment(pixel);
        },

    },
    beforeDestroy() {
        clearInterval(this.polling);
    },
    created: async function () {
        await this.getGame();
        await this.getImage();
        await this.poll();
    }
});
</script>
>
