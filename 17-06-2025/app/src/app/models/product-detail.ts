export class ProductDetailModel
{
    constructor(
        public id: number = 0,
        public title: string = "",
        public description: string = "",
        public category: string = "",
        public price: number = 0,
        public discountPercentage: number = 0,
        public rating: number = 0,
        public stock: number = 0,
        public tags: string[] = [],
        public brand: string = "",
        public weight: number = 0,
        public dimensions: { width: number; height: number; depth: number } = { width: 0, height: 0, depth: 0 },
        public warrantyInformation: string = "",
        public shippingInformation: string = "",
        public availabilityStatus: string = "",
        public reviews: {
            rating: number;
            comment: string;
            date: string;
            reviewerName: string;
            reviewerEmail: string;
        }[] = [],
        public thumbnail: string = "",
        public images: string[] = []
    ) {}
}