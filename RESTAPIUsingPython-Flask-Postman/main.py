from flask import Flask, render_template, request, make_response, jsonify

app1 = Flask(__name__)

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

@app1.route('/')
def hello_world():
    return 'Hello World!'

@app1.route('/claudio')
def hello_claudio():
    return 'Hello Claudio!'

@app1.route('/html')
def get_html():
    return render_template("index.html")

@app1.route('/qs')
def get_qs():
    if request.args:
        req=request.args
        return " ".join(f"{k}:{v}" for k, v in req.items())
    return "No Query Found!"

@app1.route("/orders")
def get_order():
    response = make_response(jsonify(order), 200)
    return response

@app1.route("/orders/<orderid>", methods=["POST"])
def post_order_details(orderid):
    req=request.get_json()
    if orderid in order:
        response = make_response(jsonify({"error":"Order ID already exists!"}), 400)
        return response
    order.update({orderid:req})
    response = make_response(jsonify({"message":"New order created"}), 201)
    return response

@app1.route("/orders/<orderid>", methods=["PUT"])
def put_order_details(orderid):
    req=request.get_json()
    if orderid in order:
        #order.update({orderid: req})
        order[orderid] = req
        response = make_response(jsonify({"message": "Order Was Update!"}), 200)
        return response
    order[orderid] = req
    response = make_response(jsonify({"message": "New Order Created!"}), 201)
    return response

@app1.route("/orders/<orderid>", methods=["PATCH"])
def patch_order_details(orderid):
    req=request.get_json()
    if orderid in order:
        for k, v in req.items():
            order[orderid][k] = v
        response = make_response(jsonify({"message": "Order Item Was Update!"}), 200)
        return response
    order[orderid] = req
    response = make_response(jsonify({"message": "New Order Created!"}), 201)
    return response

@app1.route("/orders/<orderid>", methods=["DELETE"])
def delete_order_details(orderid):
    req=request.get_json()
    if orderid in order:
        del order[orderid]
        response = make_response(jsonify({"message": "Order Deleted!"}), 204)
        return response
    response = make_response(jsonify({"error": "Order Not Located!"}), 404)
    return response

if __name__ == '__main__':
    app1.run(debug=True)