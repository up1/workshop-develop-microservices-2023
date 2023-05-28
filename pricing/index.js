"use strict";

const tracer = require("./tracer")("pricing-service");
const api = require("@opentelemetry/api");
const express = require('express')
const app = express()
const port = 3000

app.get('/', (req, res) => {
  res.send('Hello World!')
})

app.get("/healthz", (req, res) => res.status(200).send("OK"));

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`)
})