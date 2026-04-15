using CarHireSystem.Models;
using System.Threading;

namespace CarHireSystem.DataStructures;

public class BinarySearchTree : IDisposable
{
    private class Node
    {
        public Car Data { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(Car car) => Data = car;
    }

    private Node? _root;
    
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public void Insert(Car car)
    {
        if (car == null) return;

        _lock.EnterWriteLock();
        try
        {
            if (_root == null)
            {
                _root = new Node(car);
                return;
            }

            Node current = _root;
            while (true)
            {
                if (car.PricePerDay < current.Data.PricePerDay)
                {
                    if (current.Left == null) { current.Left = new Node(car); break; }
                    current = current.Left;
                }
                else
                {
                    if (current.Right == null) { current.Right = new Node(car); break; }
                    current = current.Right;
                }
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Removes the node matching <paramref name="car"/> by CarID.
    /// Handles all three BST deletion cases:
    ///   1. Leaf node            — simply removed.
    ///   2. One child            — replaced by that child.
    ///   3. Two children         — replaced with the inorder successor
    ///                             (leftmost node of the right subtree),
    ///                             then the successor is deleted from the right subtree.
    /// Because the tree is keyed on PricePerDay, equal prices go right on insert,
    /// so the same traversal direction is used when searching among duplicates.
    /// </summary>
    /// <returns>True if the car was found and removed; false if it was not in the tree.</returns>
    public bool Delete(Car car)
    {
        if (car == null) return false;

        _lock.EnterWriteLock();
        try
        {
            bool deleted = false;
            _root = DeleteNode(_root, car, ref deleted);
            return deleted;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private Node? DeleteNode(Node? node, Car car, ref bool deleted)
    {
        if (node == null) return null;

        if (car.PricePerDay < node.Data.PricePerDay)
        {
            // Target price is smaller — go left
            node.Left = DeleteNode(node.Left, car, ref deleted);
        }
        else if (car.PricePerDay > node.Data.PricePerDay)
        {
            // Target price is larger — go right
            node.Right = DeleteNode(node.Right, car, ref deleted);
        }
        else
        {
            // Prices match — check CarID to identify the exact node
            if (node.Data.Id != car.Id)
            {
                // Same price, different car. Equal prices go right on insert,
                // so the target must be somewhere in the right subtree.
                node.Right = DeleteNode(node.Right, car, ref deleted);
                return node;
            }

            // ── Found the node to delete ──────────────────────────────────────

            deleted = true;

            // Case 1: Leaf — no children, simply remove
            if (node.Left == null && node.Right == null)
                return null;

            // Case 2a: Only a right child — replace with it
            if (node.Left == null)
                return node.Right;

            // Case 2b: Only a left child — replace with it
            if (node.Right == null)
                return node.Left;

            // Case 3: Two children — find inorder successor (minimum of right subtree),
            // copy its data into this node, then delete the successor from the right subtree.
            Node successor = FindMin(node.Right);
            node.Data = successor.Data;
            node.Right = DeleteNode(node.Right, successor.Data, ref deleted);
        }

        return node;
    }

    /// <summary>Returns the leftmost (minimum-price) node in the given subtree.</summary>
    private static Node FindMin(Node node)
    {
        while (node.Left != null)
            node = node.Left;
        return node;
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _root = null;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public CarArray SearchByPriceRange(decimal min, decimal max)
    {
        _lock.EnterReadLock();
        try
        {
            CarArray results = new CarArray();
            if (min > max) return results;
            
            SearchRecursive(_root, min, max, results);
            return results;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    private void SearchRecursive(Node? node, decimal min, decimal max, CarArray results)
    {
        if (node == null) return;

        if (node.Data.PricePerDay > min)
            SearchRecursive(node.Left, min, max, results);

        if (node.Data.PricePerDay >= min && node.Data.PricePerDay <= max)
            results.Add(node.Data);

        if (node.Data.PricePerDay < max)
            SearchRecursive(node.Right, min, max, results);
    }

    public void Dispose()
    {
        _lock.Dispose();
    }
}
