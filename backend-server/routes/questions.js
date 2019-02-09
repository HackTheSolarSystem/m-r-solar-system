var express = require('express');
var router = express.Router();


router.get('/:questionNumber', function(req, res, next) {
    if(req.params.questionNumber == 1)
        res.json({ question: 'Which of these metals occurs in earths core [one]', options: ['iron', 'gold', 'nickel']});
    if(req.params.questionNumber == 2)
        res.json({ question: 'Which of these metals occurs in earths core [two]', options: ['iron', 'gold', 'nickel']});
    if(req.params.questionNumber == 3)
        res.json({ question: 'Which of these metals occurs in earths core [three]', options: ['iron', 'gold', 'nickel']});
});

module.exports = router;
