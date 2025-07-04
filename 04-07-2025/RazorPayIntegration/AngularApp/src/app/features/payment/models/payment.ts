import { OrderModel } from "../../order/models/order";

export class PaymentModel {
  constructor(
    public id: string,
    public amount?: number,
    public currency?: string,
    public status?: string,
    public razorpayPaymentId?: string,
    public order?: OrderModel,
  ) {}
}