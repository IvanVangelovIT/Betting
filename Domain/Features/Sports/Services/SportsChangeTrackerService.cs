using Soap.Models;

namespace Domain.Features.Sports.Services;

public class SportsChangeTrackerService
{
    
    public (List<T> addedItems, List<T>  removedItems, List<T>  changedItems) GetChangeTracker<T>(
        XmlSportsModel parsedCurrentSports,
        XmlSportsModel cachedPreviousSports,
        Func<XmlSportsModel, IEnumerable<T>> extractFunc,
        Func<T, int> idSelector) 
        where T : class
    {
        var currentItems = extractFunc(parsedCurrentSports).ToList();
        
        var previousItems = extractFunc(cachedPreviousSports).ToList();

        var previousItemIds = new HashSet<int>(previousItems.Select(idSelector));
        
        var currentItemIds = new HashSet<int>(currentItems.Select(idSelector));

        var addedItems = currentItems.Where(item => !previousItemIds.Contains(idSelector(item))).ToList();
        
        var removedItems = previousItems.Where(item => !currentItemIds.Contains(idSelector(item))).ToList();
        
        var changedItems = currentItems.Where(currentItem =>
            previousItems.Any(previousItem => idSelector(previousItem) == idSelector(currentItem) && !previousItem.Equals(currentItem))
        ).ToList();

        return (addedItems, removedItems, changedItems);
    }
}