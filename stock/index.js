"use strict";

const tracer = require("./tracer")("stock-service");
const api = require("@opentelemetry/api");
const mysql = require("mysql2")
const express = require('express')
const app = express()
const port = 3000

const { ExpressPrometheusMiddleware } = require('@matteodisabatino/express-prometheus-middleware')
const epm = new ExpressPrometheusMiddleware()
app.use(epm.handler)

app.get('/', (req, res) => {
  res.send('Hello World Stock Service!')
})

const db = mysql.createConnection({
  host: process.env.DATABASE_HOST,
  user: process.env.DATABASE_USER,
  password: process.env.DATABASE_PASSWORD,
  database: process.env.DATABASE
})

db.connect((error) => {
  if(error) {
      console.log(error)
  } else {
      console.log("MySQL connected!")
  }
})

app.get('/product/:id', (req, res) => {
  db.query('SELECT stock FROM product_stock WHERE product_id = ?', [req.params.id], async (error, result) => {
    if(error){
      return res.status(500).send("Error");
    }

    if(result && result.length > 0) {
      return res.json({ 
        product_id: Number(req.params.id),
        stock: result[0].stock
      });
    } 
    return res.status(404).send("Data not found");
  });
  
})

app.get("/healthz", (req, res) => res.status(200).send("OK"));

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`)
})