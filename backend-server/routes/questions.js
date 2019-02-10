var express = require('express');
var router = express.Router();


router.get('/:questionNumber', function(req, res, next) {
    if(req.params.questionNumber == 1)
        res.json({ question: 'Which of these metals occurs in earths core', options: ['iron', 'gold', 'nickel'], answer: 'iron'});
    if(req.params.questionNumber == 2)
        res.json({ question: 'What percentage of Earth\'s atmosphere is made up of Nitrogen?' , options: ['10%', '30%', '78%'], answer:'78%'});
});

module.exports = router;
