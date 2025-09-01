#region Mainetence
/*
Comment: Roles Registration motorcycle, delete and update motorcycles, new logs.
Created: 08/31/2024 21:10
Author:  Gabriel MS
*/
#endregion
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
        try
        {
            return await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Motorcycles.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Motorcycles
                .Where(m => m.LicensePlate.Contains(licensePlate))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesById(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Motorcycles
                .Where(m => m.Id.Contains(id))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Motorcycles
                .AnyAsync(m => m.LicensePlate == licensePlate, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, string excludeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Motorcycles
                .AnyAsync(m => m.LicensePlate == licensePlate && m.Id != excludeId, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Motorcycles.AddAsync(motorcycle, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> ExistMotorcycleRegistrationAsync(string licensePlate, string excludeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Rental
                .AnyAsync(m => m.MotorcycleId == excludeId, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }   
}