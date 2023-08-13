﻿using Domain.Entities.Enums;

namespace Application.Features.Boards.Dtos;

public class AddedCardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CardStatus Status { get; set; } = CardStatus.Backlog;
    public int BoardId { get; set; }
    public Color Color { get; set; } = Color.Grey;
    public DateTime CreatedDate { get; set; }
}