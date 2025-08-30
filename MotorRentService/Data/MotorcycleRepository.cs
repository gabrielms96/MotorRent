using Microsoft.EntityFrameworkCore;
using MotorRentService.Dtos;
using MotorRentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotorRentService.Data;
public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly AppDbContext _context;

    public MotorcycleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Motorcycle> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .Where(m => m.LicensePlate.Contains(licensePlate))
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesById(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .Where(m => m.Id.Contains(id))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .AnyAsync(m => m.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, string excludeId, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .AnyAsync(m => m.LicensePlate == licensePlate && m.Id != excludeId, cancellationToken);
    }

    public async Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        await _context.Motorcycles.AddAsync(motorcycle, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        _context.Motorcycles.Update(motorcycle);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        _context.Motorcycles.Remove(motorcycle);
        await _context.SaveChangesAsync(cancellationToken);
    }
} 