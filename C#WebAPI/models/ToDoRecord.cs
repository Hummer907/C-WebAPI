using System.ComponentModel.DataAnnotations;

public record ToDo(
    
    int id,
    [Required] string name, 
    DateTime dueDate, 
    bool isComplete
    
);
