from flask import Flask, render_template, request, make_response, jsonify

app = Flask(__name__)

order = {
    "order1": {
        "Size": "Small",
        "Toppings":"Cheese",
        "Crust":"Thin Crust"
    },
    "order2": {
        "Size": "Large",
        "Toppings": "Tomato",
        "Crust": "Thick Crust"
    }
}
@app.route("/orders")
def get_order():
    response = make_response(jsonify(order), 200)
    return response

@app.route("/orders/<orderid>")
def get_order_details(orderid):
    if orderid in order:
        response = make_response(jsonify(order[orderid]), 200)
        return response
    return "Order Not Foud!"

@app.route("/orders/<orderid>/<items>")
def get_order_item_details(orderid, items):
    item = order[orderid].get(items)
    if item:
        response = make_response(jsonify(item), 200)
        return response
    return "Detail of Order Not Foud!"

if __name__ == '__main__':
    app.run(debug=True)