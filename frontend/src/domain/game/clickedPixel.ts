export type dimension = {
    x: number;
    y: number;
}

export function calcRatio(displayW: number, displayH: number, originalW: number, originalH: number): dimension {
    let ratio = new Object() as dimension;
    ratio.x = originalW / displayW;
    ratio.y = originalH / displayH;
    return ratio;
}

export function clickedPixel(x: number, y: number, ratio: dimension): dimension {
    let pixel = new Object() as dimension;
    pixel.x = Math.floor(x * ratio.x);
    pixel.y = Math.floor(y * ratio.y);
    return pixel;
}