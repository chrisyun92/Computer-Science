import React from 'react';
import ReactDOM from 'react-dom';


const App = () => {
    const course = {
        name: 'Half Stack application development',
        parts: [
            {
                name: 'Fundamentals of React',
                exercises: 10
            },
            {
                name: 'Using chapter to pass data',
                exercises: 7
            },
            {
                name: 'State of a component',
                exercises: 14
            }
        ]
    }
    return (
        <>
            <Header course={course.name} />
            <Content name1={course.parts[0].name} name2={course.parts[1].name} name3={course.parts[2].name} numExercises1={course.parts[0].exercises} numExercises2={course.parts[1].exercises} numExercises3={course.parts[2].exercises} />
            <Total number={course.parts[0].exercises + course.parts[1].exercises + course.parts[2].exercises} />
        </>
    )
}

const Header = (course) => {
    return (
        <>
            <p>{course.name}</p>
        </>
    )
}

const Content = (chapter) => {
    return (
        <>
            <Part name={chapter.name1} numExercises={chapter.numExercises1} /> 
            <Part name={chapter.name2} numExercises={chapter.numExercises2} /> 
            <Part name={chapter.name3} numExercises={chapter.numExercises3} /> 
        </>
    )
}

const Part = (part) => {
    return (
        <>
            <p>{part.name} {part.numExercises}</p>
        </>
    )
}

const Total = (exercises) => {
    return (
        <>
            <p>Number of of total exercises is {exercises.number}</p>
        </>
    )
}

ReactDOM.render(<App />, document.getElementById('root'))
