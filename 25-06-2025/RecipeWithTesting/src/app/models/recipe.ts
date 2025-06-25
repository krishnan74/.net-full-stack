export class RecipeModel
{
    constructor(
        public id:number = 0,
        public name:string = "",
        public cuisine:string = "",
        public image:string = "",
        public cookTimeMinutes:number = 0,
        public ingredients:string[] = []){
    }
}