var express = require('express');
var router = express.Router();

router.get("/", (req, res, next) => {
    res.json({'earth': {'core': ['Iron', 'Nickel', 'Silicon', 'Sulfur'], 'atmosphere': [{'Nitrogen': 78.0}, {'Oxygen': 21.0}, {'Argon': 1.0}], 'mass': ['1 Me'], 'distance': ['8.32 lm']}, 'mars': {'core': ['Iron', 'Sulfur'], 'atmosphere': [{'Carbon Dioxide': 95.32}, {'Nitrogen': 2.7}, {'Argon': 1.6}], 'mass': ['0.107 Me'], 'distance': ['12.67 lm']}, 'mercury': {'core': ['Iron'], 'atmosphere': [{'Hydrogen': ''}, {'Helium': ''}, {'Oxygen': ''}], 'distance': ['3.22 lm'], 'mass': ['0.055 Me']}, 'venus': {'core': ['Iron'], 'atmosphere': [{'Carbon Dioxide': 96.0}, {'Nitrogen': 3.0}], 'distance': ['6.02 lm'], 'mass': ['0.815 Em']}});
});


module.exports = router;
